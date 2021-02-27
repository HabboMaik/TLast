using System;
using System.Security.Principal;
using System.Windows.Forms;

using Eavesdrop;

namespace TLast
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // var my = "";
            // if (my != Ud()) Environment.Exit(0);


            var overrides =
                "*ttvnw*,*edge*,*microsoft*,*bing*,*google*,*discordapp*,*gstatic.com,*imgur.com,*github.com,*googleapis.com,*facebook.com,*cloudfront.net,*gvt1.com,*jquery.com,*akamai.net,*ultra-rv.com,*youtube*,*ytimg*,*ggpht*,*images.habbo.com*";

            Eavesdropper.Certifier = new CertificateManager("Infinity", "Infinity Certificate Authority");

            Eavesdropper.Overrides.AddRange(overrides.Split(','));
            TryInstallCertificate();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
        }

        private static void TryInstallCertificate()
        {
            if (Eavesdropper.Certifier.CreateTrustedRootCertificate()) return;

            MessageBox.Show("Você precisa instalar o certificado para continuar! Caso tenha problemas tente iniciar o programa como Administrador.",
                            "Infinity - Erro de Certificado", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Environment.Exit(0);
        }

        private static string Ud()
        {
            var f         = new NTAccount(Environment.UserName);
            var s         = (SecurityIdentifier) f.Translate(typeof(SecurityIdentifier));
            var sidString = s.ToString();

            return sidString;
        }
    }
}