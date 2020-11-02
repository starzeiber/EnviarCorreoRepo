using System;
using System.Collections.Generic;

namespace EnvioCorreo
{
    /// <summary>
    /// Contiene todas las propiedades necesarias para poder enviar un correo
    /// </summary>
    public class ConfiguracionCorreo
    {
        /// <summary>
        /// Variable que almacena el usuario del correo electrónico
        /// </summary>        
        public string usuario { set; get; } = string.Empty;
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
        public bool conCertificado { set; get; } = false;
        /// <summary>
        /// Si llega una imagen de cabecera
        /// </summary>
        public bool conLogoCabecera { set; get; } = false;
        /// <summary>
        /// Ubicación de la imagen que irá en la cabecera del correo
        /// </summary>
        public string pathLogo { set; get; } = string.Empty;
        /// <summary>
        /// Variable que almacena el puerto de salida del correo electrónico, por defecto es el 587
        /// </summary>
        public int puerto { set; get; } = 587;
        /// <summary>
        /// Variable que almacena el destinatario de correo electrónico
        /// </summary>
        public List<string> listaDestinatarios { set; get; } = new List<string>();
        /// <summary>
        /// Variable que almacena el destinatario de correo electrónico en caso de error
        /// </summary>        
        public List<string> listaDestinatariosError { set; get; } = new List<string>();
        /// <summary>
        /// Variable que almacena la dirección de correo del remitente
        /// </summary>
        public string remitente { set; get; } = string.Empty;
        
    }
}
