using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SMA.Common;
using SMA.Entities;
using SMA.Models.ViewModels;
using SMA.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SMA.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    //[TypeFilter(typeof(AuthorizeIndexPageHandlerFilter))]
    public class UsuarioController : Controller
    {
        private IConfiguration Configuration;
        private string urlDomain = "http://localhost:";

        public UsuarioController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    List<TUsuario> lista = await ctx.TUsuarios.Include(x => x.IdDepartamentoNavigation).Include(x => x.IdRolNavigation).ToListAsync();
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
        public async Task<IActionResult> Create()
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    List<TDepartamento> departamentos = await ctx.TDepartamentos.ToListAsync();
                    List<TRol> roles = await ctx.TRols.ToListAsync();
                    ViewData["IdDepartamento"] = new SelectList(departamentos, "IdDepartamento", "Nombre");
                    ViewData["IdRol"] = new SelectList(roles, "IdRol", "TipoRol");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }
            return View();
        }



        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {

                TUsuario us = JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario"));



                if (id == null) return NotFound();

                if (us.IdUsuario == id)
                {
                    return RedirectToAction("UserProfile", new { id = us.IdUsuario });
                }

                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    TUsuario usuario = await ctx.TUsuarios
                        .Include(x => x.IdDepartamentoNavigation)
                        .Include(x => x.IdRolNavigation)
                        .Where(x => x.IdUsuario == id)
                        .FirstOrDefaultAsync();

                    List<TDepartamento> departamentos = await ctx.TDepartamentos.ToListAsync();
                    List<TRol> roles = await ctx.TRols.ToListAsync();



                    if (usuario == null)
                    {
                        TempData["warn"] = "No existe el registro seleccionado";
                        return RedirectToAction("Index");
                    }


                    ViewData["IdDepartamento"] = new SelectList(departamentos, "IdDepartamento", "Nombre", usuario.IdDepartamento);
                    ViewData["IdRol"] = new SelectList(roles, "IdRol", "TipoRol", usuario.IdRol);

                    return View(usuario);
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null) return NotFound();


                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    TUsuario usuario = await ctx.TUsuarios
                        .Include(x => x.IdDepartamentoNavigation)
                        .Include(x => x.IdRolNavigation)
                        .Where(x => x.IdUsuario == id)
                        .FirstOrDefaultAsync();


                    if (usuario == null)
                    {
                        TempData["warn"] = "No existe el registro seleccionado";
                        return RedirectToAction("Index");
                    }

                    return View(usuario);
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }
        }



        [Authorize(Roles = "ADMIN, REGULAR")]
        public async Task<IActionResult> UserProfile(int? id)
        {
            if (id == null) return NotFound();
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    TUsuario usuario = await ctx.TUsuarios
                        .Where(x => x.IdUsuario == id)
                        .Include(x => x.IdDepartamentoNavigation)
                        .Include(x => x.IdRolNavigation)
                        .FirstOrDefaultAsync();
                    if (usuario == null)
                    {
                        TempData["error"] = "No existe el usuario";
                        return RedirectToAction("Index", "Home");
                    }

                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Save(TUsuario usuario)
        {
            try
            {
                usuario.Estado = true;
                usuario.PrimerIngreso = true;
                usuario.FechaCreacion = DateTime.Now;
                //Autogenerar la clave
                usuario.Clave = GeneratePassword.Generar();
                usuario.Clave = Encrypt.EncrypthAES(usuario.Clave);
                if (ModelState.IsValid)
                {
                    using (BD_SMAContext ctx = new BD_SMAContext())
                    {
                        using (var transaction = ctx.Database.BeginTransaction())
                        {
                            try
                            {

                                //---------------------- VALIDAR LA NO EXISTENCIA DE CEDULA / CORREO ---------------------------//
                                TUsuario cedula = await ctx.TUsuarios.Where(x => x.Cedula == usuario.Cedula).FirstOrDefaultAsync();
                                TUsuario correo = await ctx.TUsuarios.Where(x => x.CorreoElectronico == usuario.CorreoElectronico).FirstOrDefaultAsync();

                                if (cedula != null)
                                {
                                    TempData["errorCedula"] = "Ya existe un usuario con esta cédula.";
                                }
                                if (correo != null)
                                {
                                    TempData["errorCorreo"] = "Ya existe un usuario con este correo.";
                                }
                                if (correo != null || cedula != null)
                                {
                                    List<TDepartamento> departamentos = await ctx.TDepartamentos.ToListAsync();
                                    List<TRol> roles = await ctx.TRols.ToListAsync();
                                    ViewData["IdDepartamento"] = new SelectList(departamentos, "IdDepartamento", "Nombre", usuario.IdDepartamento);
                                    ViewData["IdRol"] = new SelectList(roles, "IdRol", "TipoRol", usuario.IdRol);
                                    return View("Create", usuario);
                                }
                                //---------------------- FIN VALIDACION DE CEDULA / CORREO ---------------------------//




                                ctx.TUsuarios.Add(usuario);
                                int retorno = await ctx.SaveChangesAsync();

                                if (retorno == 0)
                                {
                                    TempData["warn"] = "Hubo un error al guardar el registro en la base de datos.";
                                    transaction.Rollback();
                                    return RedirectToAction("Index");
                                }


                                if (EnviarCorreo(usuario))
                                {
                                    TempData["success"] = "El usuario fue creado exitosamente";
                                    transaction.Commit();
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    TempData["error"] = "Hubo un error al enviar el correo electrónico de primer ingreso al usuario creado, por ende, el usuario no se guardará en la base de datos del sistema.";
                                    TempData.Keep();
                                    transaction.Rollback();
                                }
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                TempData["warn"] = "Hubo un error al guardar el registro en la base de datos.";
                                transaction.Rollback();
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
                else
                {
                    TempData["warn"] = "Hubieron errores de validación";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(TUsuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    using (BD_SMAContext ctx = new BD_SMAContext())
                    {
                        //---------------------- VALIDAR LA NO EXISTENCIA DE CEDULA / CORREO ---------------------------//
                        /*TUsuario usuarioTemp = await ctx.TUsuarios
                            .Where(x => x.IdUsuario == usuario.IdUsuario)
                            .FirstOrDefaultAsync();
                        if (usuarioTemp != null)
                        {
                            TUsuario cedula = await ctx.TUsuarios.Where(x => x.Cedula == usuario.Cedula).FirstOrDefaultAsync();
                            TUsuario correo = await ctx.TUsuarios.Where(x => x.CorreoElectronico == usuario.CorreoElectronico).FirstOrDefaultAsync();
                            bool isCedulaValid = true;
                            bool isCorreoValid = true;
                            if (usuarioTemp.Cedula != usuario.Cedula)
                            {
                                if (cedula != null && cedula.Cedula == usuario.Cedula)
                                {
                                    TempData["errorCedula"] = "Ya existe un usuario con esta cédula.";
                                    isCedulaValid = false;
                                }
                            }
                            if (usuarioTemp.CorreoElectronico != usuario.CorreoElectronico)
                            {
                                if (correo != null && correo.CorreoElectronico == usuario.CorreoElectronico)
                                {
                                    TempData["errorCorreo"] = "Ya existe un usuario con este correo.";
                                    isCorreoValid = false;
                                }
                            }

                            if(!isCedulaValid || !isCorreoValid)
                            {
                                List<TDepartamento> departamentos = await ctx.TDepartamentos.ToListAsync();
                                List<TRol> roles = await ctx.TRols.ToListAsync();
                                ViewData["IdDepartamento"] = new SelectList(departamentos, "IdDepartamento", "Nombre", usuario.IdDepartamento);
                                ViewData["IdRol"] = new SelectList(roles, "IdRol", "TipoRol", usuario.IdRol);
                                return View("Edit", usuario);
                            }
                            

                        }*/


                        //---------------------- FIN VALIDACION DE CEDULA / CORREO ---------------------------//

                        ctx.Entry(usuario).State = EntityState.Modified;

                        int retorno = await ctx.SaveChangesAsync();

                        if (retorno != 0)
                        {
                            TempData["success"] = "Éxito al actualizar!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["warn"] = "Hubo un error al actualizar";
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    TempData["warn"] = "Hubieron errores de validación";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN, REGULAR")]
        public async Task<IActionResult> EditProfile(TUsuario usuario, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (BD_SMAContext ctx = new BD_SMAContext())
                    {

                        if (file != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                await file.CopyToAsync(ms);
                                var fileBytes = ms.ToArray();
                                usuario.Foto = fileBytes;
                            }
                        }

                        TUsuario us = JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario"));
                        usuario.IdDepartamentoNavigation = us.IdDepartamentoNavigation;
                        usuario.IdRolNavigation = us.IdRolNavigation;

                        ctx.Entry(usuario).State = EntityState.Modified;

                        int retorno = await ctx.SaveChangesAsync();

                        if (retorno != 0)
                        {
                            TempData["success"] = "Éxito al actualizar!";
                            // Actualizar la sesion
                            HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(usuario));
                            //ActualizarCookies(usuario);
                            return RedirectToAction("UserProfile", new { id = usuario.IdUsuario });
                        }
                        else
                        {
                            TempData["warn"] = "Hubo un error al actualizar";
                            return RedirectToAction("UserProfile", new { id = usuario.IdUsuario });
                        }
                    }
                }
                else
                {
                    TempData["warn"] = "Hubieron errores de validación";
                    return RedirectToAction("UserProfile", new { id = usuario.IdUsuario });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("UserProfile", new { id = usuario.IdUsuario });
            }
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN, REGULAR")]
        public async Task<IActionResult> EditPassword(int idUsuario, string password)
        {
            try
            {

                using (BD_SMAContext ctx = new BD_SMAContext())
                {

                    TUsuario usuario = await ctx.TUsuarios.FindAsync(idUsuario);
                    usuario.Clave = Encrypt.EncrypthAES(password);

                    ctx.Entry(usuario).State = EntityState.Modified;

                    int retorno = await ctx.SaveChangesAsync();

                    if (retorno != 0)
                    {
                        TempData["success"] = "Éxito al actualizar la contraseña";

                        return RedirectToAction("UserProfile", new { id = idUsuario });
                    }
                    else
                    {
                        TempData["warn"] = "Hubo un error al actualizar";
                        return RedirectToAction("UserProfile", new { id = idUsuario });
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("UserProfile", new { id = idUsuario });
            }
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN, REGULAR")]
        public async Task<IActionResult> DeletePhoto(int? id)
        {
            try
            {

                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    string script = "UPDATE T_Usuarios SET foto = NULL WHERE id_usuario = " + id;
                    int retorno = await ctx.Database.ExecuteSqlRawAsync(script);
                    if (retorno == 0)
                    {
                        TempData["warn"] = "Hubo un error al eliminar la imagen";
                        return RedirectToAction("UserProfile", new { id = id });
                    }
                    var us = (JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario")));
                    us.Foto = null;
                    HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(us));

                    TempData["success"] = "La imagen fue eliminada exitosamente";
                    return RedirectToAction("UserProfile", new { id = id });

                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("UserProfile", new { id = id });
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {

                using (BD_SMAContext ctx = new BD_SMAContext())
                {

                    TUsuario usuario = await ctx.TUsuarios
                        .Where(x => x.IdUsuario == id)
                        .Include(x => x.TTickets)
                        .FirstOrDefaultAsync();

                    if (usuario == null)
                    {
                        TempData["warn"] = "Registro no existente";
                        return RedirectToAction("Index");
                    }

                    usuario.Estado = false;
                    ctx.Entry(usuario).State = EntityState.Modified;
                    int retorno = await ctx.SaveChangesAsync();

                    if (retorno != 0)
                    {
                        TempData["success"] = "Éxito al desactivar!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["warn"] = "Hubo un error al eliminar";
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }

        }


        private bool EnviarCorreo(TUsuario usuario)
        {
            try
            {
                string correoEmisor = this.Configuration["CredencialesCorreo:email"];
                string clave = this.Configuration["CredencialesCorreo:password"];
                var request = HttpContext.Request.Host;
                string url = urlDomain + request.Port + "/" + "Login";
                MailMessage mmsg = new MailMessage();
                mmsg.To.Add(new MailAddress(usuario.CorreoElectronico));
                mmsg.Subject = "Bienvenido a SMA - Contraseña para primer ingreso";
                mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
                string ClaveTemp = Encrypt.DecrypthAES(usuario.Clave);
                mmsg.Body = "<p>¡Hola " + usuario.Nombre + " " + usuario.Apellidos + ", te damos la bienvenida a nuestro sistema!</p><br><br><p>Su registro ha sido correcto. Se le asigó la contraseña: </p><p style='font-weight: bold'>" + ClaveTemp + "</p><br><br><p>Para iniciar sesión </p><a href='" + url + "'>presione aquí</a><br><br><p>Recuerde que cuando ingrese se le solicitará realizar un cambio de contraseña para brindar una mayor seguridad a su cuenta.</p><br><br><p>¡Gracias por usar nuestro sistema!</p><br><br><img src=\"https://www.soportecompleto.com/images/icons/herramientas.png\" width =\"40%\" height=\"10%\"/>";
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
            catch (Exception)
            {

                return false;
            }
        }



        public void ActualizarCookies(TUsuario usuario)
        {
            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                                new Claim("Correo", usuario.CorreoElectronico),
                                new Claim(ClaimTypes.Role, usuario.IdRolNavigation.TipoRol)
                            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
        }



    }
}
