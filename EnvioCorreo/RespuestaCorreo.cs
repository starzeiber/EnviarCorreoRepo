using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnvioCorreo
{
    /// <summary>
    /// Clase que contiene las propiedades de la respuesta al envío de un correo
    /// </summary>
    public class RespuestaCorreo
    {
        /// <summary>
        /// Variable que indica si el proceso fue exitoso
        /// </summary>
        public Boolean esExitoso = true;
        /// <summary>
        /// Si existe un error, en esta variable se describe
        /// </summary>
        public String DescripcionError = String.Empty;
    }
}
