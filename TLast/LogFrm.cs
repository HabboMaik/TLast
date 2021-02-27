using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TLast
{
    public partial class LogFrm : Form
    {
        public const int          WM_NCLBUTTONDOWN = 0xA1;
        public const int          HT_CAPTION       = 0x2;
        public       Action<bool> AntiAFK;

        public Action TopMostChanged;

        public LogFrm()
        {
            InitializeComponent();
        }

        #region Controls

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void picClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        public void ShowHide()
        {
            if (Visible) Hide();
            else Show();
        }

        private void ckbTopMost_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = ckbTopMost.Checked;
            TopMostChanged?.Invoke();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AntiAFK?.Invoke(checkBox1.Checked);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void AppendText(string text, Color color)
        {
            richTextBox1.SelectionStart  = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;

            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(text);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
        }

        public void LogInfo(string text)
        {
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            AppendText($"[{DateTime.Now.ToShortTimeString()}] ", Color.Aqua);
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            AppendText(text + Environment.NewLine, Color.Aqua);
        }

        public void LogWarning(string text)
        {
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            AppendText($"[{DateTime.Now.ToShortTimeString()}] ", Color.Yellow);
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            AppendText(text + Environment.NewLine, Color.Yellow);
        }

        public void LogError(string text)
        {
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            AppendText($"[{DateTime.Now.ToShortTimeString()}] ", Color.Red);
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            AppendText(text + Environment.NewLine, Color.Red);
        }

        public void LogSuccess(string text)
        {
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            AppendText($"[{DateTime.Now.ToShortTimeString()}] ", Color.Lime);
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            AppendText(text + Environment.NewLine, Color.Lime);
        }

        #endregion
    }
}