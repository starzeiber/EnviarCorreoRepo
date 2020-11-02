using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace EnvioCorreo
{
    /// <summary>
    /// Contiene las funciones para poder enviar un correo
    /// </summary>
    public class EnviarCorreo
    {
        /// <summary>
        /// Función que envía un correo incluyendo una tabla
        /// </summary>
        /// <param name="configuracionCorreo">Parámetros del correo</param>
        /// <param name="titulo">Título del correo</param>        
        /// <param name="listaElementosTabla">Se utiliza para enviar un correo en forma de grid, cada elemento del listado se considera como un renglón del grid. 
        /// el número de celdas será dividas por pipes (|); Ej: celda1|celda2|celda3|.....|
        /// para tener formato de TITULO en la tabla, deberá concatenarse la palabra "Titulo" al contenido de la misma. Cuando se quiera un color rojo de fuente, 
        /// deberá concatenarse la palabra "Rojo" al elemento; de lo contrario, el elemento no llevará formato adicional. Ej: campoUnoTitulo|campoDosTitulo|...
        ///  valorCelda1Rojo|valorCelda2|valorCelda3Rojo...</param> 
        /// <returns></returns>
        public RespuestaCorreo EnvioCorreo(ConfiguracionCorreo configuracionCorreo, string titulo, List<string> listaElementosTabla)
        {
            String mensajeHtml = ObtenerHtml(listaElementosTabla);

            MailMessage mensajeCorreo = new MailMessage(configuracionCorreo.remitente, configuracionCorreo.listaDestinatarios[0], titulo, mensajeHtml);

            SmtpClient clienteSmtp = new SmtpClient(configuracionCorreo.smtp);

            mensajeCorreo.IsBodyHtml = true;

            if (configuracionCorreo.conLogoCabecera)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mensajeHtml, null, MediaTypeNames.Text.Html);
                LinkedResource logo = new LinkedResource(configuracionCorreo.pathLogo);
                logo.ContentId = "Logo";
                htmlView.LinkedResources.Add(logo);
                mensajeCorreo.AlternateViews.Add(htmlView);
            }

            for (int i = 1; i < configuracionCorreo.listaDestinatarios.Count; i++)
            {
                mensajeCorreo.CC.Add(configuracionCorreo.listaDestinatarios[i]);
            }

            clienteSmtp.Credentials = new System.Net.NetworkCredential(configuracionCorreo.usuario, configuracionCorreo.pass);
            clienteSmtp.Port = configuracionCorreo.puerto;
            if (configuracionCorreo.conCertificado)
            {
                clienteSmtp.EnableSsl = true;
            }
            else
            {
                clienteSmtp.EnableSsl = false;
            }


            try
            {
                clienteSmtp.Send(mensajeCorreo);
                return new RespuestaCorreo() { esExitoso = true };
            }
            catch (Exception ex)
            {
                return new RespuestaCorreo() { esExitoso = false, DescripcionError = ex.Message };
            }
        }

        /// <summary>
        /// Función que envía un correo solo de texto plano
        /// </summary>
        /// <param name="configuracionCorreo">Parámetros del correo</param>
        /// <param name="titulo">Título del correo</param> 
        /// <param name="mensaje">Mensaje en el cuerpo del correo</param>
        /// <param name="esError">Se marca para enviar un correo con formato de error</param>
        /// <returns></returns>
        public RespuestaCorreo EnvioCorreo(ConfiguracionCorreo configuracionCorreo, string titulo, string mensaje, Boolean esError)
        {
            String mensajeHtml = ObtenerHtml(mensaje);
            MailMessage mensajeCorreo;
            if (esError == true)
            {
                mensajeCorreo = new MailMessage(configuracionCorreo.remitente, configuracionCorreo.listaDestinatarios[0], titulo + " Error", mensajeHtml);
                mensajeCorreo.Priority = MailPriority.High;
            }
            else
            {
                mensajeCorreo = new MailMessage(configuracionCorreo.remitente, configuracionCorreo.listaDestinatarios[0], titulo, mensajeHtml);
            }
            SmtpClient clienteSmtp = new SmtpClient(configuracionCorreo.smtp);

            mensajeCorreo.IsBodyHtml = true;

            if (configuracionCorreo.conLogoCabecera)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mensajeHtml, null, MediaTypeNames.Text.Html);
                LinkedResource logo = new LinkedResource(configuracionCorreo.pathLogo);
                logo.ContentId = "Logo";
                htmlView.LinkedResources.Add(logo);
                mensajeCorreo.AlternateViews.Add(htmlView);
            }

            foreach (String cadaElemento in configuracionCorreo.listaDestinatarios)
            {
                mensajeCorreo.CC.Add(cadaElemento);
            }

            clienteSmtp.Credentials = new System.Net.NetworkCredential(configuracionCorreo.usuario, configuracionCorreo.pass);
            clienteSmtp.Port = configuracionCorreo.puerto;
            if (configuracionCorreo.conCertificado)
            {
                clienteSmtp.EnableSsl = true;
            }
            else
            {
                clienteSmtp.EnableSsl = false;
            }

            try
            {
                clienteSmtp.Send(mensajeCorreo);
                return new RespuestaCorreo();
            }
            catch (Exception ex)
            {
                return new RespuestaCorreo() { esExitoso = false, DescripcionError = ex.Message };
            }
        }

        /// <summary>
        /// Función que entrega el html cuando es una tabla para ser enviado por correo electrónico
        /// </summary>
        /// <param name="listaElementosTabla">Lista de elementos de la tabla, se considera cada renglón como un elemento dividido por pipes (|) sus celdas, 
        /// cuando es título, deberá concatenarse la palabra Titulo. Cuando se marque con color rojo, deberá concatenarse la palabra Rojo al elemento, 
        /// de lo contrario el elemento no llevará formato adicional. Ej: campoUnoTitulo|campoDosTitulo|..." </param>            
        /// <returns></returns>        
        private string ObtenerHtml(List<string> listaElementosTabla)
        {
            try
            {
                String tabla = "<table style='width: 100%; '>";
                foreach (String cadaRenglon in listaElementosTabla)
                {
                    String[] elementosSplit = cadaRenglon.Split('|');
                    if (elementosSplit.Length > 0)
                    {
                        tabla = tabla + "<tr>";
                        for (int i = 0; i < elementosSplit.Length; i++)
                        {
                            if (elementosSplit[i].IndexOf("Titulo") > 0)
                            {
                                tabla = tabla + "<td class='titulo' >" + elementosSplit[i].Substring(0, elementosSplit[i].Length - 6) + "</td>";
                            }
                            else if (elementosSplit[i].IndexOf("Rojo") > 0)
                            {
                                tabla = tabla + "<td class='bajo' >" + elementosSplit[i].Substring(0, elementosSplit[i].Length - 4) + "</td>";
                            }
                            else
                            {
                                tabla = tabla + "<td class='alto' >" + elementosSplit[i] + "</td>";
                            }
                        }
                    }
                    tabla = tabla + "</tr>";
                }
                tabla = tabla + "</table>";


                StringBuilder constructorHtml = new StringBuilder();
                constructorHtml.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
                constructorHtml.Append("<html xmlns='http://www.w3.org/1999/xhtml' >");
                constructorHtml.Append("<head><title>Untitled Page</title>");
                constructorHtml.Append("<style type='text / css'> .bajo {  color: #FFFFFF;  text-align: center;  border-bottom: 1px solid #0000FF; background-color: #FF0000;   } .alto {  color: #0000FF; text-align: center;  border-bottom: 1px solid #0000FF;} table {  border: 1px solid red;  } img{display:block;margin:auto;}  .titulo { background-color: #0000FF; color: #FFFFFF; text-align: center;  border-bottom: 1px solid #0000FF; font-weight: bold}  </style>");
                constructorHtml.Append("</ head > ");
                constructorHtml.Append("<body><img class='centrado' src=\"cid:Logo\">");
                constructorHtml.Append(tabla);
                constructorHtml.Append("</body></html>");
                return constructorHtml.ToString();
            }
            catch (Exception ex)
            {
                return "Error creando el mensaje de correo" + ex.Message;
            }
        }

        /// <summary>
        /// Función que obtiene el html de un mensaje de texto plano
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        private string ObtenerHtml(string mensaje)
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
    }
}