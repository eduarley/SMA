using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SMA.Common;
using SMA.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace SMA.Controllers
{
    public class TicketController : Controller
    {
        private IConfiguration Configuration;
        string urlDomain = "http://localhost:";

        public TicketController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        [Authorize(Roles = "ADMIN,REGULAR")]
        public async Task<IActionResult> Index()
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {

                    List<TTicket> lista = new List<TTicket>();

                    //TUsuario usuario = JsonConvert.DeserializeObject<TUsuario>(User.FindFirst("oUsuario").Value);
                    TUsuario usuario = JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario"));
                    if (usuario != null)
                    {
                        if (String.Equals(usuario.IdRolNavigation.TipoRol, "admin", StringComparison.OrdinalIgnoreCase))
                        {
                            lista = await ctx.TTickets
                                .OrderByDescending(x => x.Fecha)
                                .Include(x => x.IdUsuarioNavigation)
                                .Include(x => x.IdPrioridadNavigation)
                                .ToListAsync();



                            return View("TicketsAll", lista);
                        }
                        else
                        {
                            lista = await ctx.TTickets
                                .OrderByDescending(x => x.Fecha)
                                .Include(x => x.IdUsuarioNavigation)
                                .Include(x => x.IdPrioridadNavigation)
                                .Where(x => x.IdUsuario == usuario.IdUsuario).ToListAsync();
                            return View("MyTickets", lista);
                        }

                    }

                    return View(lista);
                }
            }
            catch (Exception ex)
            {
                @ViewData["Error"] = "Hubo un error: " + ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }




        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DetailsReview(int? id)
        {
            try
            {
                if (id == null) return NotFound();


                using (BD_SMAContext ctx = new BD_SMAContext())
                {

                    TTicket ticket = await ctx.TTickets
                        .Where(x => x.NumTicket == id)
                        .Include(x => x.TArchivos)
                        .Include(x => x.IdPrioridadNavigation)
                        .Include(x => x.IdRevisionNavigation)
                        .Include(x => x.IdUsuarioNavigation)
                        .FirstOrDefaultAsync();



                    if (ticket == null)
                    {
                        TempData["warn"] = "No existe el registro seleccionado";
                        return RedirectToAction("Index");
                    }
                    List<TPrioridadTicket> prioridad = await ctx.TPrioridadTickets.ToListAsync();
                    List<TEstadoAceptacion> estado = await ctx.TEstadoAceptacions.ToListAsync();
                    ViewData["IdPrioridad"] = new SelectList(prioridad, "IdPrioridad", "TipoPrioridad");
                    ViewData["IdEstado"] = new SelectList(estado, "IdEstado", "Tipo");
                    ViewBag.Fecha = ticket.Fecha.ToString("dd MMMM yyyy hh:mm tt");
                    return View(ticket);
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }
        }




        [Authorize(Roles = "REGULAR")]
        public async Task<IActionResult> DetailsReviewReadOnly(int? id)
        {
            try
            {
                if (id == null) return NotFound();


                using (BD_SMAContext ctx = new BD_SMAContext())
                {

                    TTicket ticket = await ctx.TTickets
                        .Where(x => x.NumTicket == id)
                        .Include(x => x.TArchivos)
                        .Include(x => x.IdPrioridadNavigation)
                        .Include(x => x.IdRevisionNavigation)
                        .Include(x => x.IdRevisionNavigation.IdEstadoNavigation)
                        .Include(x => x.IdUsuarioNavigation)
                        .FirstOrDefaultAsync();



                    if (ticket == null)
                    {
                        TempData["warn"] = "No existe el registro seleccionado";
                        return RedirectToAction("Index");
                    }
                    List<TPrioridadTicket> prioridad = await ctx.TPrioridadTickets.ToListAsync();
                    ViewData["IdPrioridad"] = new SelectList(prioridad, "IdPrioridad", "TipoPrioridad");
                    ViewBag.Fecha = ticket.Fecha.ToString("dd MMMM yyyy hh:mm tt");
                    return View(ticket);
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }
        }



        [Authorize(Roles = "REGULAR")]
        public async Task<IActionResult> Create()
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    List<TPrioridadTicket> prioridad = await ctx.TPrioridadTickets.ToListAsync();
                    ViewData["IdPrioridad"] = new SelectList(prioridad, "IdPrioridad", "TipoPrioridad");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "REGULAR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TTicket ticket, List<IFormFile> files)
        {
            TUsuario usuario;
            try
            {
                usuario = JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario"));
                //ticket.IdUsuario = int.Parse(User.FindFirst("Id").Value);

                ticket.IdUsuario = usuario.IdUsuario;
                ticket.Fecha = DateTime.Now;
                ticket.Revisado = false;
                List<TArchivo> archivos = new List<TArchivo>();





                if (ModelState.IsValid)
                {
                    using (BD_SMAContext ctx = new BD_SMAContext())
                    {
                        using (var transaction = ctx.Database.BeginTransaction())
                        {
                            try
                            {

                                if (files != null)
                                {
                                    using (var ms = new MemoryStream())
                                    {
                                        foreach (var item in files)
                                        {
                                            TArchivo archivo = new TArchivo();
                                            await item.CopyToAsync(ms);
                                            var fileBytes = ms.ToArray();
                                            archivo.Archivo = fileBytes;
                                            archivo.Nombre = item.FileName;
                                            archivo.Extension = item.ContentType;
                                            archivos.Add(archivo);
                                            ticket.TArchivos.Add(archivo);
                                        }

                                    }

                                }


                                ctx.TTickets.Add(ticket);
                                int retorno = await ctx.SaveChangesAsync();
                                if (retorno == 0)
                                {
                                    transaction.Rollback();
                                    TempData["warn"] = "Hubo un error al generar la notificación.";
                                }


                                transaction.Commit();
                                TempData["success"] = "El ticket fue creado exitosamente.";

                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                TempData["error"] = "Error fatal: " + ex.Message;
                            }
                        }

                    }
                }
                else
                {
                    TempData["warn"] = "Hubieron errores de validación";
                    return RedirectToAction("Create");

                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }


            if (usuario != null && ticket != null)
            {
                CrearNotificacion(ticket, usuario);
                //GenerateXML(ticket.NumTicket);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SaveRevision(int? IdTicket, string Observacion, int IdPrioridad, int IdEstado)
        {
            TUsuario usuario = null;
            TTicket ticket = null;
            TRevisionTicket revisionTicket = null;
            try
            {
                usuario = JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario"));

                if (ModelState.IsValid)
                {
                    using (BD_SMAContext ctx = new BD_SMAContext())
                    {
                        using (var transaction = ctx.Database.BeginTransaction())
                        {
                            try
                            {


                                if (IdTicket == null || string.IsNullOrEmpty(Observacion) || IdPrioridad == 0 || IdEstado == 0)
                                {
                                    TempData["warn"] = "Hubieron errores de validación";
                                    return RedirectToAction("Index");
                                }

                                revisionTicket = new TRevisionTicket()
                                {
                                    //IdUsuario = int.Parse(User.FindFirst("Id").Value),
                                    IdUsuario = JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario")).IdUsuario,
                                    IdPrioridad = IdPrioridad,
                                    IdEstado = IdEstado,
                                    Observacion = Observacion,
                                    FechaRevision = DateTime.Now
                                };



                                ticket = ctx.TTickets.Find(IdTicket);
                                ticket.IdRevisionNavigation = revisionTicket;
                                ticket.Revisado = true;


                                ctx.Entry(ticket).State = EntityState.Modified;
                                ctx.TRevisionTickets.Add(revisionTicket);

                                int retorno = await ctx.SaveChangesAsync();
                                if (retorno == 0)
                                {
                                    transaction.Rollback();
                                    TempData["warn"] = "Hubo un error al guardar el registro en la base de datos.";
                                }


                                transaction.Commit();
                                TempData["success"] = "Se registró exitosamente.";

                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                TempData["error"] = "Error fatal: " + ex.Message;
                            }
                        }

                    }
                }
                else
                {
                    TempData["warn"] = "Hubieron errores de validación";

                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }



            if (usuario != null && ticket != null && revisionTicket != null)
            {
                CrearNotificacion(ticket, usuario);
                SendMail(ticket, revisionTicket.IdRevision);
            }
            /*if (usuario != null && ticket != null && revisionTicket != null)
            {
                SendMail(usuario, ticket, revisionTicket);
            }*/

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN,REGULAR")]
        public async Task<IActionResult> DownloadFile(int? idArchivo)
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {

                    TArchivo archivo = await ctx.TArchivos.FindAsync(idArchivo);
                    if (archivo == null)
                    {
                        TempData["warn"] = "No existe el archivo a descargar";
                        return RedirectToAction("Index");
                    }

                    return new FileContentResult(archivo.Archivo, archivo.Extension)
                    {
                        FileDownloadName = archivo.Nombre
                    };
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal al descargar: " + ex.Message;
                return RedirectToAction("Index");
            }
        }



        private bool CrearNotificacion(TTicket ticket, TUsuario usuario)
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    TNotificacionGeneral notificacionGeneral;
                    if (!string.Equals(usuario.IdRolNavigation.TipoRol, "admin", StringComparison.OrdinalIgnoreCase))
                    {
                        notificacionGeneral = new TNotificacionGeneral()
                        {
                            Titulo = "Nuevo ticket",
                            Descripcion = usuario.Nombre + " " + usuario.Apellidos + " ha enviado un nuevo ticket.",
                            NumTicket = ticket.NumTicket,
                            Fecha = DateTime.Now,
                            IdEmisor = usuario.IdUsuario,
                            Visto = false
                        };
                    }
                    else
                    {
                        notificacionGeneral = new TNotificacionGeneral()
                        {
                            Titulo = "Nueva revisión",
                            Descripcion = usuario.Nombre + " " + usuario.Apellidos + " realizó una revisión a tu ticket.",
                            NumTicket = ticket.NumTicket,
                            Fecha = DateTime.Now,
                            IdEmisor = usuario.IdUsuario,
                            Visto = false,
                            IdReceptor = ticket.IdUsuario
                        };
                    }


                    ctx.TNotificacionGenerals.Add(notificacionGeneral);
                    int retorno = ctx.SaveChanges();
                    if (retorno == 0)
                        return false;
                    else
                        return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        private void GenerateXML(int numTicket)
        {
            try
            {
                TTicket ticket = null;
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    ticket = ctx.TTickets
                       .Include(x => x.IdUsuarioNavigation)
                       .Include(x => x.IdUsuarioNavigation.IdDepartamentoNavigation)
                       .Include(x => x.IdUsuarioNavigation.IdRolNavigation)
                       .Include(x => x.IdPrioridadNavigation)
                       .Where(x => x.NumTicket == numTicket)
                       .FirstOrDefault();
                }
                if (ticket != null)
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    doc.AppendChild(docNode);

                    XmlElement ticketDataNode = doc.CreateElement("TicketDataNode");
                    (ticketDataNode).SetAttribute("xmls:xsi", "http://www.abc.com");
                    (ticketDataNode).SetAttribute("schemaLocation", "http://www.abc.com/XMLSchema-instance", "http://www.abc.com/ aaa.xsd");
                    (ticketDataNode).SetAttribute("xmls", "http://www.abc.com");
                    doc.AppendChild(ticketDataNode);



                    XmlNode headerNode = doc.CreateElement("Header");
                    ticketDataNode.AppendChild(headerNode);


                    XmlNode numTicketNode = doc.CreateElement("Numero");
                    numTicketNode.AppendChild(doc.CreateTextNode(ticket.NumTicket.ToString()));
                    headerNode.AppendChild(numTicketNode);

                    XmlNode fechaNode = doc.CreateElement("Fecha");
                    fechaNode.AppendChild(doc.CreateTextNode(ticket.Fecha.ToString("dd-MM-yyyy hh:mm tt")));
                    headerNode.AppendChild(fechaNode);


                    XmlNode bodyNode = doc.CreateElement("Body");
                    ticketDataNode.AppendChild(bodyNode);

                    XmlNode tituloNode = doc.CreateElement("Titulo");
                    tituloNode.AppendChild(doc.CreateTextNode(ticket.Titulo));
                    bodyNode.AppendChild(tituloNode);

                    XmlNode descripcionNode = doc.CreateElement("Descripcion");
                    descripcionNode.AppendChild(doc.CreateTextNode(ticket.Descripcion));
                    bodyNode.AppendChild(descripcionNode);


                    XmlNode prioridadNode = doc.CreateElement("Prioridad");
                    prioridadNode.AppendChild(doc.CreateTextNode(ticket.IdPrioridadNavigation.TipoPrioridad));
                    bodyNode.AppendChild(prioridadNode);



                    XmlNode propietarioNode = doc.CreateElement("Propietario");
                    bodyNode.AppendChild(propietarioNode);

                    XmlNode usuarioNombreNode = doc.CreateElement("Nombre");
                    usuarioNombreNode.AppendChild(doc.CreateTextNode(ticket.IdUsuarioNavigation.Nombre));
                    propietarioNode.AppendChild(usuarioNombreNode);


                    XmlNode usuarioApellidosNode = doc.CreateElement("Apellidos");
                    usuarioApellidosNode.AppendChild(doc.CreateTextNode(ticket.IdUsuarioNavigation.Apellidos));
                    propietarioNode.AppendChild(usuarioApellidosNode);


                    XmlNode usuarioCedulaNode = doc.CreateElement("Cedula");
                    usuarioCedulaNode.AppendChild(doc.CreateTextNode(ticket.IdUsuarioNavigation.Cedula));
                    propietarioNode.AppendChild(usuarioCedulaNode);


                    XmlNode usuarioCorreoNode = doc.CreateElement("Correo");
                    usuarioCorreoNode.AppendChild(doc.CreateTextNode(ticket.IdUsuarioNavigation.CorreoElectronico));
                    propietarioNode.AppendChild(usuarioCorreoNode);


                    XmlNode usuarioDeptoNode = doc.CreateElement("Departamento");
                    usuarioDeptoNode.AppendChild(doc.CreateTextNode(ticket.IdUsuarioNavigation.IdDepartamentoNavigation.Nombre));
                    propietarioNode.AppendChild(usuarioDeptoNode);


                    XmlNode usuarioRolNode = doc.CreateElement("Rol");
                    usuarioRolNode.AppendChild(doc.CreateTextNode(ticket.IdUsuarioNavigation.IdRolNavigation.TipoRol));
                    propietarioNode.AppendChild(usuarioRolNode);





                    var basePath = Path.Combine(Environment.CurrentDirectory, @"XMLFiles\");
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    var newFileName = string.Format("{0}{1}", Guid.NewGuid().ToString("N"), ".xml");
                    doc.Save(basePath + newFileName);
                }

            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private bool SendMail(TTicket ticket, int idRevision)
        {
            try
            {

                TRevisionTicket revisionTicket = null;
                TUsuario usuario = null;
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    revisionTicket = ctx
                        .TRevisionTickets
                        .Include(x => x.IdUsuarioNavigation)
                        .Include(x => x.IdPrioridadNavigation)
                        .Include(x => x.IdEstadoNavigation)
                        .Where(x => x.IdRevision == idRevision)
                        .FirstOrDefault();

                    usuario = ctx
                        .TUsuarios
                        .Where(x => x.IdUsuario == ticket.IdUsuario)
                        .FirstOrDefault();
                }

                if(revisionTicket != null && usuario != null)
                {
                    string correoEmisor = this.Configuration["CredencialesCorreo:email"];
                    string clave = this.Configuration["CredencialesCorreo:password"];
                    MailMessage mmsg = new MailMessage();
                    mmsg.To.Add(new MailAddress(usuario.CorreoElectronico));
                    mmsg.Subject = "Nueva revisión";
                    mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
                    mmsg.Body = FormatoCorreoHTML.CuerpoCorreoRevision(usuario, ticket, revisionTicket);
                    mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                    mmsg.IsBodyHtml = true;
                    mmsg.From = new MailAddress(correoEmisor, "Soporte SMA");
                    SmtpClient smtp = new SmtpClient();
                    smtp.Credentials = new NetworkCredential(correoEmisor, clave);
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Send(mmsg);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        

    }
}
