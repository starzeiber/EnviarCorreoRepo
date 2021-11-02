using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace MailOperation
{
    /// <summary>
    /// Contiene las funciones para poder enviar un correo
    /// </summary>
    public class Mail
    {
        private readonly MailConfig mailConfig;
        public Mail(MailConfig mailConfig)
        {
            this.mailConfig = mailConfig;
            if (!CheckParameters())
            {
                throw new Exception("Uno o varios valores incorrectos en ConfiguracionCorreo");
            }
        }

        public string GetHtmlBasic(string mensaje)
        {
            try
            {
                StringBuilder constructorHtml = new StringBuilder();
                constructorHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
                constructorHtml.Append("<html xmlns='http://www.w3.org/1999/xhtml' >");
                constructorHtml.Append("<head><title>Untitled Page</title>");
                constructorHtml.Append("<style type='text / css'> .bajo {  color: #FFFFFF;  text-align: center;  border-bottom: 1px solid #0000FF; background-color: #FF0000;   } .alto {  color: #0000FF; text-align: center;  border-bottom: 1px solid #0000FF;} table {  border: 1px solid red;  } img{display:block;margin:auto;}  .titulo { background-color: #0000FF; color: #FFFFFF; text-align: center;  border-bottom: 1px solid #0000FF; font-weight: bold}  </style>");
                constructorHtml.Append("</ head > ");
                constructorHtml.Append("<body><img class='centrado' src=\"cid:Logo\">");
                constructorHtml.Append(mensaje);
                constructorHtml.Append("</body></html>");
                return constructorHtml.ToString();
            }
            catch (Exception ex)
            {
                return "Error creando el mensaje de correo" + ex.Message;
            }
        }

        public MailResponse SendMail(string title, string html)
        {
            MailResponse mailResponse = new MailResponse();
            string mensajeHtml = GetHtmlBasic(html);
            MailMessage mailMessage = new MailMessage(mailConfig.sender, mailConfig.listRecipient.First(), title, mensajeHtml);

            SmtpClient clienteSmtp = new SmtpClient(mailConfig.smtp);
            if (mailConfig.withHighPriority)
                mailMessage.Priority = MailPriority.High;
            mailMessage.IsBodyHtml = true;
            
            try
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mensajeHtml, null, MediaTypeNames.Text.Html);
                mailMessage.AlternateViews.Add(htmlView);

                if (mailConfig.withAttachment)
                {
                    //LinkedResource logo = new LinkedResource(configuracionCorreo.pathLogo);
                    //logo.ContentId = "Logo";
                    //htmlView.LinkedResources.Add(logo);
                    if (File.Exists(mailConfig.pathAttachment))
                    {
                        // Create  the file attachment for this email message.
                        Attachment data = new Attachment(mailConfig.pathAttachment, MediaTypeNames.Application.Octet);
                        // Add time stamp information for the file.
                        ContentDisposition disposition = data.ContentDisposition;
                        disposition.CreationDate = System.IO.File.GetCreationTime(mailConfig.pathAttachment);
                        disposition.ModificationDate = System.IO.File.GetLastWriteTime(mailConfig.pathAttachment);
                        disposition.ReadDate = System.IO.File.GetLastAccessTime(mailConfig.pathAttachment);
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

                if (mailConfig.listRecipient.Count>1)
                {
                    mailConfig.listRecipient.RemoveAt(0);
                    foreach (string item in mailConfig.listRecipient)
                    {
                        mailMessage.CC.Add(item);
                    }
                }

                clienteSmtp.Credentials = new System.Net.NetworkCredential(mailConfig.user, mailConfig.pass);
                clienteSmtp.Port = mailConfig.port;
                if (mailConfig.withCertificateSSL)
                {
                    clienteSmtp.EnableSsl = true;
                }
                else
                {
                    clienteSmtp.EnableSsl = false;
                }

                clienteSmtp.Send(mailMessage);
                mailResponse.success = true;
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