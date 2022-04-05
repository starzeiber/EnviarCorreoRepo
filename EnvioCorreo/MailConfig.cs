using System.Collections.Generic;

namespace MailOperation
{
    /// <summary>
    /// Contiene todas las propiedades necesarias para poder enviar un correo
    /// </summary>
    public class MailConfig
    {
        /// <summary>
        /// Variable que almacena el usuario del correo electrónico
        /// </summary>        
        public string user { set; get; } = string.Empty;
        /// <summary>
        /// Variable que almacena el smtp del correo electrónico
        /// </summary>
        public string smtp { set; get; } = string.Empty;
        /// <summary>
        /// Variable que almacena el password del correo electrónico
        /// </summary>
        public string pass { set; get; } = string.Empty;
        /// <summary>
        /// Variable que indica si el SMTP necesita certificado de seguridad
        /// </summary>
        public bool withCertificateSSL { set; get; } = false;
        /// <summary>
        /// Variable que almacena el puerto de salida del correo electrónico, por defecto es el 587
        /// </summary>
        public int port { set; get; } = 587;
        /// <summary>
        /// Variable que almacena la dirección de correo del remitente
        /// </summary>
        public string sender { set; get; } = string.Empty;
        /// <summary>
        /// Variable que almacena el destinatario de correo electrónico
        /// </summary>
        public List<string> listRecipient { set; get; }
        /// <summary>
        /// Con prioridad alta
        /// </summary>
        public bool withHighPriority { get; set; }
        /// <summary>
        /// indica que llevará un adjunto
        /// </summary>
        public bool withAttachment { get; set; }
        ///// <summary>
        ///// Ruta del archivo adjunto
        ///// </summary>
        //public string pathAttachment { get; set; }
        /// <summary>
        /// Listado de las rutas de los archivos a adjuntar
        /// </summary>
        public List<string> listPathAttachments { set; get; }

        public MailConfig()
        {
            user = "";
            smtp = "";
            pass = "";
            withCertificateSSL = false;
            withHighPriority = false;
            port = 0;
            listRecipient = new List<string>();
            sender = "";
            withAttachment = false;
            listPathAttachments = new List<string>();
        }

    }
}
