using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;

namespace Cliente
{
    public partial class Form1 : Form
    {
        public Boolean CargarConfiguracionCorreo(EnvioCorreo.ConfiguracionCorreo configuracionCorreo)
        {
            try
            {
                configuracionCorreo.usuario = ConfigurationManager.AppSettings["usuario"];
                configuracionCorreo.smtp = ConfigurationManager.AppSettings["smtp"];
                configuracionCorreo.pass = ConfigurationManager.AppSettings["pass"];
                configuracionCorreo.puerto = int.Parse(ConfigurationManager.AppSettings["puerto"]);
                configuracionCorreo.remitente = configuracionCorreo.usuario;
                configuracionCorreo.conCertificado = true;
                int numeroDestinatarios = int.Parse(ConfigurationManager.AppSettings["numeroDestinatarios"]);
                for (int i = 1; i <= numeroDestinatarios; i++)
                {
                    configuracionCorreo.listaDestinatarios.Add(ConfigurationManager.AppSettings["destinatario" + i.ToString()]);
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
            EnvioCorreo.ConfiguracionCorreo configuracionCorreo = new EnvioCorreo.ConfiguracionCorreo();
            EnvioCorreo.RespuestaCorreo respuestaCorreo;
            if (CargarConfiguracionCorreo(configuracionCorreo))
            {
                EnvioCorreo.EnviarCorreo enviarCorreo = new EnvioCorreo.EnviarCorreo();
                respuestaCorreo = enviarCorreo.EnvioCorreo(configuracionCorreo, "es una prueba", "prueba",false);
                if (respuestaCorreo.esExitoso != true)
                {
                    MessageBox.Show(respuestaCorreo.DescripcionError);
                }
            }
        }
        
    }
}
