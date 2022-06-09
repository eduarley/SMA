using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMA.Common;
using SMA.Entities;
using SMA.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SMA.Controllers
{
    public class RecoveryController : Controller
    {
        private IConfiguration Configuration;
        string urlDomain = "http://localhost:";

        public RecoveryController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> StartRecovery(RecoveryPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (BD_SMAContext ctx = new BD_SMAContext())
                    {
                        TUsuario usuario = await ctx.TUsuarios
                            .Where(x => x.Cedula == model.Cedula && x.CorreoElectronico == model.Correo)
                            .FirstOrDefaultAsync();
                        if (usuario != null)
                        {
                            string token = Encrypt.EncryptToken(Guid.NewGuid().ToString());
                            usuario.Token = token;

                            ctx.Entry(usuario).State = EntityState.Modified;
                            int retorno = await ctx.SaveChangesAsync();
                            if (retorno == 0)
                            {
                                TempData["warn"] = "Error al generar y guardar el token.";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                if (!SendMail(usuario))
                                {
                                    TempData["warn"] = "Error al enviar correo.";
                                    return RedirectToAction("Index");
                                }
                            }

                        }
                        else
                        {
                            TempData["warn"] = "No se encontró nungún usuario con esos datos.";
                            return RedirectToAction("Index");
                        }

                    }
                }

                TempData["success"] = "Se ha enviado un correo electrónico con los pasos a seguir para recuperar su contraseña";
                return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                TempData["warn"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }
        }



        public async Task<IActionResult> RecoveryPassword(string token)
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                   
                    TUsuario usuario = await ctx.TUsuarios.Where(x => x.Token == token).FirstOrDefaultAsync();
                    if(usuario != null)
                    {
                        ChangePasswordViewModel viewModel = new ChangePasswordViewModel();
                        viewModel.Token = token;
                        return View(viewModel);
                    }
                    else
                    {
                        TempData["error"] = "No existe token relacionado o token ha expirado.";
                        return RedirectToAction("Index", "Login");
                    }
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
        public async Task<IActionResult> RecoveryConfirmed(ChangePasswordViewModel viewModel)
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    
                    TUsuario usuario = await ctx.TUsuarios.Where(x => x.Token == viewModel.Token).FirstOrDefaultAsync();
                    if (usuario != null)
                    {
                        usuario.Token = null;
                        usuario.Clave = Encrypt.EncrypthAES(viewModel.Password);
                        ctx.Entry(usuario).State = EntityState.Modified;
                        int retorno = await ctx.SaveChangesAsync();
                        if (retorno == 0)
                        {
                            TempData["warn"] = "Hubo un problema al actualizar la contraseña.";
                            return RedirectToAction("Index", "Login");
                        }
                        TempData["success"] = "Contraseña actualizada correctamente. Ingrese sus nuevas credenciales";
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        TempData["warn"] = "No se encontró nungún usuario con esos datos.";
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

        private bool SendMail(TUsuario usuario)
        {
            try
            {

                string correoEmisor = this.Configuration["CredencialesCorreo:email"];
                string clave = this.Configuration["CredencialesCorreo:password"];
                var request = HttpContext.Request.Host;
                string url = urlDomain + request.Port + "/" + "Recovery/RecoveryPassword/?token=" + usuario.Token;
                MailMessage mmsg = new MailMessage();
                mmsg.To.Add(new MailAddress(usuario.CorreoElectronico));
                mmsg.Subject = "Bienvenido a SMA - Recuperación de contraseña";
                mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
                string ClaveTemp = Encrypt.DecrypthAES(usuario.Clave);
                mmsg.Body = "<p>Saludos!</p><br><br><p>Adjuntamos su enlace de recuperación de contraseña:</p><br><br><a href='" + url + "'>Ingrese aquí para recuperar su contraseña</a><br><br><p>¡Gracias por usar nuestro sistema!</p><br><br><img src=\"https://www.soportecompleto.com/images/icons/herramientas.png\" width =\"40%\" height=\"10%\"/>";
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

    }
}
