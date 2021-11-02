using System;
using System.Configuration;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Form1 : Form
    {
        public Boolean CargarConfiguracionCorreo(MailOperation.MailConfig configuracionCorreo)
        {
            try
            {
                configuracionCorreo.user = ConfigurationManager.AppSettings["usuario"];
                configuracionCorreo.smtp = ConfigurationManager.AppSettings["smtp"];
                configuracionCorreo.pass = ConfigurationManager.AppSettings["pass"];
                configuracionCorreo.port = int.Parse(ConfigurationManager.AppSettings["puerto"]);
                configuracionCorreo.sender = configuracionCorreo.user;
                configuracionCorreo.withCertificateSSL = true;
                int numeroDestinatarios = int.Parse(ConfigurationManager.AppSettings["numeroDestinatarios"]);
                for (int i = 1; i <= numeroDestinatarios; i++)
                {
                    configuracionCorreo.listRecipient.Add(ConfigurationManager.AppSettings["destinatario" + i.ToString()]);
                }
                int numeroDestinatariosError = int.Parse(ConfigurationManager.AppSettings["numeroDestinatariosError"]);
                for (int i = 1; i <= numeroDestinatariosError; i++)
                {
                    configuracionCorreo.listaDestinatariosError.Add(ConfigurationManager.AppSettings["destinatarioError" + i.ToString()]);
                }
                configuracionCorreo.pathLogo = ConfigurationManager.AppSettings["cabecera"];
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MailOperation.MailConfig configuracionCorreo = new MailOperation.MailConfig();
            MailOperation.MailResponse respuestaCorreo;
            if (CargarConfiguracionCorreo(configuracionCorreo))
            {
                MailOperation.Mail enviarCorreo = new MailOperation.EnviarCorreo();
                respuestaCorreo = enviarCorreo.EnvioCorreo(configuracionCorreo, "es una prueba", "prueba", false);
                if (respuestaCorreo.success != true)
                {
                    MessageBox.Show(respuestaCorreo.errorDescription);
                }
            }
        }

    }
}
