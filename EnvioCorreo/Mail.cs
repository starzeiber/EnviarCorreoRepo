using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;

namespace MailOperation
{
    /// <summary>
    /// Contiene las funciones para poder enviar un correo
    /// </summary>
    public class Mail
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
            if (!CheckParameters())
            {
                throw new Exception("Uno o varios valores incorrectos en ConfiguracionCorreo");
            }
        }

        /// <summary>
        /// Envia un correo por un cliente smtp
        /// </summary>
        /// <param name="title">titulo del correo</param>
        /// <param name="html">html que será utilizado en el cuerpo del correo</param>
        /// <returns></returns>
        public MailResponse SendMail(string title, string html)
        {
            MailResponse mailResponse = new MailResponse();
            
            MailMessage mailMessage = new MailMessage(mailConfig.sender, mailConfig.listRecipient.First(), title, html);

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

                if (mailConfig.listRecipient.Count > 1)
                {
                    mailConfig.listRecipient.RemoveAt(0);
                    foreach (string item in mailConfig.listRecipient)
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

        private bool CheckParameters()
        {
            if (mailConfig.user == "" || mailConfig.smtp == "" || mailConfig.pass == "" || mailConfig.port == 0 || mailConfig.listRecipient.Count == 0 || mailConfig.sender == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}