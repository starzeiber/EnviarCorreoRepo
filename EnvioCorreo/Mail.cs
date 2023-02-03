using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MailOperation
{
    /// <summary>
    /// Contiene las funciones para poder enviar un correo
    /// </summary>
    public interface IMail
    {
        /// <summary>
        /// Envia un correo por un cliente smtp
        /// </summary>
        /// <param name="title">titulo del correo</param>
        /// <param name="html">html que será utilizado en el cuerpo del correo</param>
        /// <returns>instancia de MailResponse</returns>
        MailResponse SendMail(string title, string html);
    }

    /// <summary>
    /// Contiene las funciones para poder enviar un correo
    /// </summary>
    public class Mail : IMail
    {
        private const int timeOut = 30000;

        private readonly MailConfig mailConfig;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="mailConfig">Configuración del correo</param>
        public Mail(MailConfig mailConfig)
        {
            this.mailConfig = mailConfig;
        }

        /// <summary>
        /// Envia un correo por un cliente smtp
        /// </summary>
        /// <param name="title">titulo del correo</param>
        /// <param name="html">html que será utilizado en el cuerpo del correo</param>
        /// <returns></returns>
        public MailResponse SendMail(string title, string html)
        {
            if (!CheckParameters(title, html))
            {
                return new MailResponse()
                {
                    success = false,
                    errorDescription = "Uno o varios valores incorrectos en la configuracion de correo"
                };
            }

            MailResponse mailResponse = new MailResponse();

            MailMessage mailMessage = new MailMessage(mailConfig.sender, mailConfig.listReceptors.First(), title, html);

            if (mailConfig.withHighPriority)
                mailMessage.Priority = MailPriority.High;
            mailMessage.IsBodyHtml = true;

            try
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
                mailMessage.AlternateViews.Add(htmlView);

                if (mailConfig.withAttachment)
                {
                    foreach (string eachAttachment in mailConfig.listPathAttachments)
                    {
                        if (File.Exists(eachAttachment))
                        {
                            // Create  the file attachment for this email message.
                            Attachment data = new Attachment(eachAttachment, MediaTypeNames.Application.Octet);
                            // Add time stamp information for the file.
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(eachAttachment);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(eachAttachment);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(eachAttachment);
                            // Add the file attachment to this email message.
                            mailMessage.Attachments.Add(data);
                        }
                        else
                        {
                            mailResponse.errorDescription = "No existe el archivo adjunto";
                            mailResponse.success = false;
                            return mailResponse;
                        }
                    }
                    //LinkedResource logo = new LinkedResource(configuracionCorreo.pathLogo);
                    //logo.ContentId = "Logo";
                    //htmlView.LinkedResources.Add(logo);
                    //if (File.Exists(mailConfig.pathAttachment))
                    //{
                    //    // Create  the file attachment for this email message.
                    //    Attachment data = new Attachment(mailConfig.pathAttachment, MediaTypeNames.Application.Octet);
                    //    // Add time stamp information for the file.
                    //    ContentDisposition disposition = data.ContentDisposition;
                    //    disposition.CreationDate = System.IO.File.GetCreationTime(mailConfig.pathAttachment);
                    //    disposition.ModificationDate = System.IO.File.GetLastWriteTime(mailConfig.pathAttachment);
                    //    disposition.ReadDate = System.IO.File.GetLastAccessTime(mailConfig.pathAttachment);
                    //    // Add the file attachment to this email message.
                    //    mailMessage.Attachments.Add(data);
                    //}
                    //else
                    //{
                    //    mailResponse.errorDescription = "No existe el archivo adjunto";
                    //    mailResponse.success = false;
                    //    return mailResponse;
                    //}
                }

                if (mailConfig.listReceptors.Count > 1)
                {
                    mailConfig.listReceptors.RemoveAt(0);
                    foreach (string item in mailConfig.listReceptors)
                    {
                        mailMessage.CC.Add(item);
                    }
                }

                SmtpClient smtpClient = new SmtpClient(mailConfig.smtp);
                smtpClient.Timeout = timeOut;
                smtpClient.Credentials = new System.Net.NetworkCredential(mailConfig.user, mailConfig.pass);
                smtpClient.Port = mailConfig.port;
                smtpClient.EnableSsl = mailConfig.withCertificateSSL;

                smtpClient.Send(mailMessage);
                mailResponse.success = true;
                return mailResponse;
            }
            catch (SmtpException smtpException)
            {
                mailResponse.success = false;
                mailResponse.errorDescription = smtpException.Message;
                return mailResponse;
            }
            catch (Exception ex)
            {
                mailResponse.success = false;
                mailResponse.errorDescription = ex.Message;
                return mailResponse;
            }
        }

        /// <summary>
        /// Envia un correo por un cliente smtp
        /// </summary>
        /// <param name="title">titulo del correo</param>
        /// <param name="html">html que será utilizado en el cuerpo del correo</param>
        /// <returns></returns>
        public async Task<MailResponse> SendMailAsync(string title, string html)
        {
            if (!CheckParameters(title, html))
            {
                return new MailResponse()
                {

                    success = false,
                    errorDescription = "Uno o varios valores incorrectos en la configuracion de correo"

                };
            }

            MailResponse mailResponse = new MailResponse();

            MailMessage mailMessage = new MailMessage(mailConfig.sender, mailConfig.listReceptors.First(), title, html);

            if (mailConfig.withHighPriority)
                mailMessage.Priority = MailPriority.High;
            mailMessage.IsBodyHtml = true;

            try
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
                mailMessage.AlternateViews.Add(htmlView);

                if (mailConfig.withAttachment)
                {
                    foreach (string eachAttachment in mailConfig.listPathAttachments)
                    {
                        if (File.Exists(eachAttachment))
                        {
                            // Create  the file attachment for this email message.
                            Attachment data = new Attachment(eachAttachment, MediaTypeNames.Application.Octet);
                            // Add time stamp information for the file.
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(eachAttachment);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(eachAttachment);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(eachAttachment);
                            // Add the file attachment to this email message.
                            mailMessage.Attachments.Add(data);
                        }
                        else
                        {
                            mailResponse.errorDescription = "No existe el archivo adjunto";
                            mailResponse.success = false;
                            return mailResponse;
                        }
                    }
                    //LinkedResource logo = new LinkedResource(configuracionCorreo.pathLogo);
                    //logo.ContentId = "Logo";
                    //htmlView.LinkedResources.Add(logo);
                    //if (File.Exists(mailConfig.pathAttachment))
                    //{
                    //    // Create  the file attachment for this email message.
                    //    Attachment data = new Attachment(mailConfig.pathAttachment, MediaTypeNames.Application.Octet);
                    //    // Add time stamp information for the file.
                    //    ContentDisposition disposition = data.ContentDisposition;
                    //    disposition.CreationDate = System.IO.File.GetCreationTime(mailConfig.pathAttachment);
                    //    disposition.ModificationDate = System.IO.File.GetLastWriteTime(mailConfig.pathAttachment);
                    //    disposition.ReadDate = System.IO.File.GetLastAccessTime(mailConfig.pathAttachment);
                    //    // Add the file attachment to this email message.
                    //    mailMessage.Attachments.Add(data);
                    //}
                    //else
                    //{
                    //    mailResponse.errorDescription = "No existe el archivo adjunto";
                    //    mailResponse.success = false;
                    //    return mailResponse;
                    //}
                }

                if (mailConfig.listReceptors.Count > 1)
                {
                    mailConfig.listReceptors.RemoveAt(0);
                    foreach (string item in mailConfig.listReceptors)
                    {
                        mailMessage.CC.Add(item);
                    }
                }

                SmtpClient smtpClient = new SmtpClient(mailConfig.smtp);
                smtpClient.Timeout = timeOut;
                smtpClient.Credentials = new System.Net.NetworkCredential(mailConfig.user, mailConfig.pass);
                smtpClient.Port = mailConfig.port;
                smtpClient.EnableSsl = mailConfig.withCertificateSSL;

                //smtpClient.Send(mailMessage);
                await smtpClient.SendMailAsync(mailMessage);
                mailResponse.success = true;
                return mailResponse;
            }
            catch (SmtpException smtpException)
            {
                mailResponse.success = false;
                mailResponse.errorDescription = smtpException.Message;
                return mailResponse;
            }
            catch (Exception ex)
            {
                mailResponse.success = false;
                mailResponse.errorDescription = ex.Message;
                return mailResponse;
            }
        }

        private bool CheckParameters(string title, string html)
        {
            try
            {
                if (string.IsNullOrEmpty(mailConfig.user) ||
                string.IsNullOrEmpty(mailConfig.smtp) ||
                string.IsNullOrEmpty(mailConfig.pass) ||
                mailConfig.port == 0 ||
                mailConfig.listReceptors.Count == 0 ||
                string.IsNullOrEmpty(mailConfig.sender) ||
                string.IsNullOrEmpty(html) ||
                string.IsNullOrEmpty(title))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}