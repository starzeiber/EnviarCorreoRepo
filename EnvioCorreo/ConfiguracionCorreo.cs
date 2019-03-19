using System;
using System.Collections.Generic;

namespace EnvioCorreo
{
    public class ConfiguracionCorreo
    {
        /// <summary>
        /// Variable que almacena el usuario del correo electrónico
        /// </summary>        
        public String usuario = String.Empty;
        /// <summary>
        /// Variable que almacena el smtp del correo electrónico
        /// </summary>
        public String smtp = String.Empty;
        /// <summary>
        /// Variable que almacena el password del correo electrónico
        /// </summary>
        public String pass = String.Empty;
        /// <summary>
        /// Variable que indica si el SMTP necesita certificado de seguridad
        /// </summary>
        public Boolean conCertificado = false;
        /// <summary>
        /// Si llega una imagen de cabecera
        /// </summary>
        public Boolean conLogoCabecera = false;
        /// <summary>
        /// Variable que almacena el puerto de salida del correo electrónico
        /// </summary>
        public int puerto = 587;
        /// <summary>
        /// Variable que almacena el destinatario de correo electrónico
        /// </summary>
        public List<String> listaDestinatarios = new List<string>();
        /// <summary>
        /// Variable que almacena el destinatario de correo electrónico en caso de error
        /// </summary>        
        public List<String> listaDestinatariosError = new List<string>();
        /// <summary>
        /// Variable que almacena la dirección de correo del remitente
        /// </summary>
        public String remitente = String.Empty;
        /// <summary>
        /// Ubicación de la imagen que irá en la cabecera del correo
        /// </summary>
        public String pathLogo = String.Empty;
    }
}
