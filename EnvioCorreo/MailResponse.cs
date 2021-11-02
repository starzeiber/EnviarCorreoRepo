namespace MailOperation
{
    /// <summary>
    /// Clase que contiene las propiedades de la respuesta al envío de un correo
    /// </summary>
    public class MailResponse
    {
        /// <summary>
        /// Variable que indica si el proceso fue exitoso
        /// </summary>
        public bool success = true;
        /// <summary>
        /// Si existe un error, en esta variable se describe
        /// </summary>
        public string errorDescription = string.Empty;
    }
}
