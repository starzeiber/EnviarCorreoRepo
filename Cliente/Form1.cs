using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UMB.MailOperation.MailConfig mailConfig = new UMB.MailOperation.MailConfig();
            UMB.MailOperation.MailResponse mailResponse;
            if (LoadConfiguration(mailConfig))
            {
                UMB.MailOperation.Mail mail = new UMB.MailOperation.Mail(mailConfig);

                mailResponse = mail.SendMail("es una prueba", "prueba");
                if (mailResponse.success != true)
                {
                    MessageBox.Show(mailResponse.errorDescription);
                }
                else
                {
                    MessageBox.Show("correo enviado");
                }
            }
        }

        private bool LoadConfiguration(UMB.MailOperation.MailConfig mailConfig)
        {
            try
            {
                mailConfig.user = ConfigurationManager.AppSettings["usuario"];
                mailConfig.smtp = ConfigurationManager.AppSettings["smtp"];
                mailConfig.pass = ConfigurationManager.AppSettings["pass"];
                mailConfig.withCertificateSSL = true;
                mailConfig.port = int.Parse(ConfigurationManager.AppSettings["puerto"]);
                mailConfig.sender = mailConfig.user;
                int numeroDestinatarios = int.Parse(ConfigurationManager.AppSettings["numeroDestinatarios"]);
                for (int i = 1; i <= numeroDestinatarios; i++)
                {
                    mailConfig.listRecipient.Add(ConfigurationManager.AppSettings["destinatario" + i.ToString()]);
                }
                mailConfig.withHighPriority = true;
                mailConfig.withAttachment = true;
                mailConfig.pathAttachment = "C:/prueba.pdf";
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string ObtenerHtmlCorreo()
        {
            string fileName;
            string path;
            string cadena;
            try
            {
                fileName = "index.html";
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plantillas", fileName);
                cadena = File.ReadAllText(path);
                return cadena;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
    }
}
