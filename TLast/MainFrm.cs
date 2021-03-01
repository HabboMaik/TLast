using System;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

using Eavesdrop;

using Newtonsoft.Json;

using TLast.Models;

namespace TLast
{
    public partial class MainFrm : Form
    {
        private const string PRODUCT_VERSION_API =
            "https://images.habbo.com/habbo-webgl-clients/205_3887bb9ab2bd85a393c1c2e5162dec1b/WebGL/habbo2020-global-prod/Build/habbo2020-global-prod.json";

        private const string USER_API = "https://www.habbo.com.br/api/public/users?name={0}";

        //form move
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION       = 0x2;

        private readonly BotHandler _botHandler;

        private readonly LogFrm _logFrm;

        public MainFrm()
        {
            InitializeComponent();
            _logFrm                     =  new LogFrm();
            _logFrm.TopMostChanged      += () => TopMost = _logFrm.TopMost;
            _botHandler                 =  new BotHandler(_logFrm);
            _botHandler.BotCountUpdated += BotCountUpdated;

            cbboxGender.SelectedIndex = 0;

            // var my = "S-1-5-21-809661726-2335831991-902902274-1000";
            // if (my != Ud()) Environment.Exit(0);
        }

        private static string Ud()
        {
            var f         = new NTAccount(Environment.UserName);
            var s         = (SecurityIdentifier) f.Translate(typeof(SecurityIdentifier));
            var sidString = s.ToString();

            return sidString;
        }

        private void BotCountUpdated(int count)
        {
            Invoke((MethodInvoker) delegate { lblConnectedAccounts.Text = $"Contas Conectadas: {count}"; });
        }

        private async void MainFrm_Load(object sender, EventArgs e)
        {
            var (product, success) = await TryGetModelByUrlTaskAsync<ProductVersionModel>(PRODUCT_VERSION_API);

            if (success)
                Config.ProductVersion = product.ProductVersion;
            else
            {
                MessageBox.Show("Ocorreu um erro ao baixar informações de versão do jogo, verifique sua conexão com a internet ou cantate o desenvolvedor.",
                                "Infinity - Alerta!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Environment.Exit(0);
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            _logFrm.ShowHide();
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRoomId.Text, out var roomId)) _botHandler.JoinRoom(roomId);
        }

        private async void btnCopyFigureString_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFigure.Text)) return;

            var (user, success) = await TryGetModelByUrlTaskAsync<UserModel>(string.Format(USER_API, txtFigure.Text));

            if (!success) return;

            _botHandler.ChangeFigure(user.FigureString, cbboxGender.Text);
        }

        private async Task<(T model, bool success)> TryGetModelByUrlTaskAsync<T>(string url)
        {
            try
            {
                var client = new WebClient
                {
                    Headers =
                    {
                        [HttpRequestHeader.UserAgent] =
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36"
                    }
                };


                var data = await client.DownloadStringTaskAsync(url);

                var model = JsonConvert.DeserializeObject<T>(data);

                return (model, true);
            }
            catch
            {
                return ((T) default, false);
            }
        }

        #region MoveForm

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TerminateProxy();

            Environment.Exit(0);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        #region Proxy

        private void TerminateProxy()
        {
            if (!Eavesdropper.IsRunning) return;

            Eavesdropper.Terminate();
            Eavesdropper.ResponseInterceptedAsync -= CaptureSSOTokenAsync;
        }

        private void StartProxy()
        {
            if (Eavesdropper.IsRunning) return;

            Eavesdropper.ResponseInterceptedAsync += CaptureSSOTokenAsync;
            Eavesdropper.Initiate(2727);
        }

        private async Task CaptureSSOTokenAsync(object sender, ResponseInterceptedEventArgs e)
        {
            if (e.Uri.AbsoluteUri == "https://www.habbo.com.br/api/client/clientv2url")
            {
                // #if DEBUG
                //                 MessageBox.Show(e.Uri.AbsoluteUri);
                // #endif
                var content = await e.Content.ReadAsStringAsync();
                var split   = content.Split('/');
                var sso     = split[^1].Replace("\"}", string.Empty);

                _botHandler.AddBot(sso);

                e.Cancel = true;
            }
        }

        private void lblServerStatus_Click(object sender, EventArgs e)
        {
            if (lblServerStatus.Text == "Não")
            {
                StartProxy();
                lblServerStatus.Text      = "Sim";
                lblServerStatus.ForeColor = Color.Lime;
            }
            else
            {
                TerminateProxy();
                lblServerStatus.Text      = "Não";
                lblServerStatus.ForeColor = Color.Red;
            }
        }

        #endregion
    }
}