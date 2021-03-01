using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

using TLast.Models;

using WebSocketSharp.Server;

namespace TLast
{
    public partial class MainFrm : Form
    {
        private const string PRODUCT_VERSION_API =
            "https://images.habbo.com/habbo-webgl-clients/205_3887bb9ab2bd85a393c1c2e5162dec1b/WebGL/habbo2020-global-prod/Build/habbo2020-global-prod.json";

        private const string USER_API = "https://www.habbo.com.br/api/public/users?name={0}";

        //form move
        public const     int                 WM_NCLBUTTONDOWN = 0xA1;
        public const     int                 HT_CAPTION       = 0x2;
        private  Queue<AccountModel> _accounts;

        private readonly BotHandler _botHandler;

        private readonly LogFrm _logFrm;

        private readonly WebSocketServer _socketServer;

        public MainFrm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            _logFrm                     =  new LogFrm();
            _logFrm.TopMostChanged      += () => TopMost = _logFrm.TopMost;
            _botHandler                 =  new BotHandler(_logFrm);
            _botHandler.BotCountUpdated += BotCountUpdated;

            cbboxGender.SelectedIndex = 0;

            _socketServer = new WebSocketServer(27);
            // _socketServer.AddWebSocketService<CaptchaService>(new CaptchaService());

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

            var accounts = await File.ReadAllLinesAsync("contas.txt");

            if (accounts.Length == 0) return;

            var validAccounts = new List<AccountModel>();

            foreach (var account in accounts)
            {
                var acc = new AccountModel(account);
                if (!acc.IsValid) continue;

                validAccounts.Add(acc);
            }

            _accounts = new Queue<AccountModel>(validAccounts);
            _socketServer.AddWebSocketService("/ric", () => new CaptchaService(_accounts, _botHandler, _logFrm));
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

        private void lblServerStatus_Click(object sender, EventArgs e)
        {
            if (lblServerStatus.Text == "Não")
            {
                _socketServer.Start();
                lblServerStatus.Text      = "Sim";
                lblServerStatus.ForeColor = Color.Lime;
            }
            else
            {
                _socketServer.Stop();
                lblServerStatus.Text      = "Não";
                lblServerStatus.ForeColor = Color.Red;
            }
        }

        #region MoveForm

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (_socketServer.IsListening) _socketServer.Stop();

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
    }
}