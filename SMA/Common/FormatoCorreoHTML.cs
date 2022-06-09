using SMA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMA.Common
{
    public static class FormatoCorreoHTML
    {
        public static string CuerpoCorreoRevision(TUsuario usuario, TTicket ticket, TRevisionTicket revisionTicket)
        {

            string nombreReceptor = usuario.Nombre;
            string nombreEmisor = revisionTicket.IdUsuarioNavigation.Nombre + " " + revisionTicket.IdUsuarioNavigation.Apellidos;
            string fechaTicket = ticket.Fecha.ToString("dd/MM/yyyy hh:mm tt");
            string tituloTicket = ticket.Titulo;
            string descripcionTicket = ticket.Descripcion;

            string prioridadRevision = revisionTicket.IdPrioridadNavigation.TipoPrioridad;
            string estadoRevision = revisionTicket.IdEstadoNavigation.Tipo;
            string observacionRevision = revisionTicket.Observacion;
            string fechaRevision = revisionTicket.FechaRevision.ToString("dd/MM/yyyy hh:mm tt");


            string respuesta = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional //EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                <html
                  xmlns='http://www.w3.org/1999/xhtml'
                  xmlns:o='urn:schemas-microsoft-com:office:office'
                  xmlns:v='urn:schemas-microsoft-com:vml'
                  lang='en'
                >
                  <head>
                    <link
                      rel='stylesheet'
                      type='text/css'
                      hs-webfonts='true'
                      href='https://fonts.googleapis.com/css?family=Lato|Lato:i,b,bi'
                    />
                    <title>Revisión</title>
                    <meta property='og:title' content='Revisión' />

                    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />

                    <meta http-equiv='X-UA-Compatible' content='IE=edge' />

                    <meta name='viewport' content='width=device-width, initial-scale=1.0' />

                    <style type='text/css'>
                      a {
                        text-decoration: underline;
                        color: inherit;
                        font-weight: bold;
                        color: #253342;
                      }

                      h1 {
                        font-size: 56px;
                      }

                      h2 {
                        font-size: 28px;
                        font-weight: 900;
                      }

                      p {
                        font-weight: 100;
                        text-align: justify;
                      }

                      td {
                        vertical-align: top;
                      }

                      #email {
                        margin: auto;
                        width: 600px;
                        background-color: white;
                      }

                      .button {
                        font: inherit;
                        background-color: #ff7a59;
                        border: none;
                        padding: 10px;
                        text-transform: uppercase;
                        letter-spacing: 2px;
                        font-weight: 900;
                        color: white;
                        border-radius: 5px;
                        box-shadow: 3px 3px #d94c53;
                        text-decoration: none;
                      }

                      .subtle-link {
                        font-size: 9px;
                        text-transform: uppercase;
                        letter-spacing: 1px;
                        color: #cbd6e2;
                      }

                      label {
                        font-weight: bold;
                      }
                    </style>
                  </head>

                  <body
                    bgcolor='#F5F8FA'
                    style='
                      width: 100%;
                      margin: auto 0;
                      padding: 0;
                      font-family: Lato, sans-serif;
                      font-size: 18px;
                      color: #33475b;
                      word-break: break-word;
                    '
                  >
                    <div id='email'>
                      <! Banner -->
                      <table role='presentation' width='100%'>
                        <tr>
                          <td bgcolor='#00A4BD' align='center' style='color: white'>
                            <img
                              alt='handshake'
                              src='https://cdn-icons-png.flaticon.com/512/1006/1006555.png'
                              width='150px'
                              align='middle'
                            />

                            <h1>Hola " + nombreReceptor + @"!</h1>
                          </td>
                        </tr>
                      </table>

                      <! First Row -->

                      <table
                        role='presentation'
                        border='0'
                        cellpadding='0'
                        cellspacing='10px'
                        style='padding: 30px 30px 30px 60px'
                      >
                        <tr>
                          <td>
                            <h2>Una nueva revisión</h2>
                            <p>
                              " + nombreEmisor + @" ha realizado una nueva revisión al ticket que realizaste
                              el dia " + fechaTicket + @". Los detalles de la revisión se indicarán más
                              adelante en este mismo correo.
                            </p>
                          </td>
                        </tr>
                      </table>

                      <! Second Row with Two Columns-->

                      <table
                        role='presentation'
                        border='0'
                        cellpadding='0'
                        cellspacing='10px'
                        width='100%'
                        style='padding: 30px 30px 30px 60px'
                      >
                        <tr>
                          <td>
                            <img
                              alt='ticket'
                              src='https://cdn-icons-png.flaticon.com/512/2942/2942934.png'
                              width='150px'
                              align='middle'
                            />

                            <h2>Tu ticket</h2>

                            <label>Titulo:</label>
                            <p>" + tituloTicket + @"</p>

                            <label>Descripción:</label>
                            <p>" + descripcionTicket + @"</p>

                            <label>Prioridad:</label>
                            <p>Baja</p>

                            <label>Fecha de emisión:</label>
                            <p>" + fechaTicket + @"</p>
                          </td>

                          <td>
                            <img
                              alt='revision'
                              src='https://cdn-icons-png.flaticon.com/512/1632/1632670.png'
                              width='150px'
                              align='middle'
                            />
                            <h2>Su revisión</h2>

                            <label>Prioridad:</label>
                            <p>" + prioridadRevision + @"</p>

                            <label>Estado:</label>
                            <p>" + estadoRevision + @"</p>

                            <label>Observaciones:</label>
                            <p>" + observacionRevision + @"</p>

                            <label>Fecha de revisión:</label>
                            <p>" + fechaRevision + @"</p>
                          </td>
                        </tr>

                        
                      </table>

                      <! Banner Row -->
                      <table
                        role='presentation'
                        bgcolor='#EAF0F6'
                        width='100%'
                        style='margin-top: 50px'
                      >
                        <tr>
                          <td align='center' style='padding: 30px 30px'>
                            <h2>SMA</h2>
                            <p style='text-align: center'>Sistema de Mesa de Ayuda</p>
                          </td>
                        </tr>
                      </table>
                    </div>
                  </body>
                </html>
";
            return respuesta;


        }
    }
}
