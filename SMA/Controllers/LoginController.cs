using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SMA.Common;
using SMA.Entities;
using SMA.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SMA.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                if (User.Identity.IsAuthenticated) 
                { 
                    return RedirectToAction("Index", "Principal"); 
                }
                else 
                { 
                    return View(); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    using (BD_SMAContext ctx = new BD_SMAContext())
                    {

                        string encrypt = Encrypt.EncrypthAES(model.Clave);

                        TUsuario usuario = await ctx.TUsuarios

                            .Where(x => x.CorreoElectronico == model.Correo && x.Clave == encrypt)
                            .FirstOrDefaultAsync();

                        //List<TNotificacionGeneral> notificacionesGenerales = await ctx.TNotificacionGenerals.ToListAsync();

                        //ViewData["Cantidad"] = await ctx.TNotificacionGenerals.CountAsync();

                        if (usuario != null)
                        {

                            if (!usuario.Estado)
                            {
                                TempData["mensaje"] = "Este usuario está inactivo";
                                return View("index");
                            }


                            //-------- sin esto se encicla al agregarle al session
                            TRol rol = ctx.TRols.Find(usuario.IdRol);
                            rol.TUsuarios = null;
                            usuario.IdRolNavigation = rol;


                            /*TDepartamento dep = ctx.TDepartamentos.Find(usuario.IdDepartamento);
                            dep.TUsuarios = null;
                            usuario.IdDepartamentoNavigation = dep;*/


                            //--------



                            //claims para guardar el usario en cookies
                            AutenticarCookies(usuario);


                            //Guardar en session
                            HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(usuario));



                            if (usuario.PrimerIngreso)
                            {
                                return RedirectToAction("ChangePassword", new { id = usuario.IdUsuario, token = usuario.Clave });
                            }


                            return RedirectToAction("Index", "Principal");


                        }
                        else
                        {
                            TempData["mensaje"] = "Correo electrónico o clave incorrectos";
                            return View("Index");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Hubo un problema: " + ex.Message;
                return RedirectToAction("index");
            }
            return View("index");
        }



        public IActionResult ChangePassword(int id, string token)
        {

            if (string.IsNullOrEmpty(token)) return NotFound();


            try
            {


                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    TUsuario usuario = ctx.TUsuarios.Find(id);
                    if (usuario != null)
                    {
                        if (usuario.Clave == token)
                        {
                            ChangePasswordViewModel model = new ChangePasswordViewModel();
                            model.IdUsuario = id;
                            return View(model);
                        }
                    }
                }



                TempData["error"] = "Error al encontrar usuario";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ChangePasswordConfirmed(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (BD_SMAContext ctx = new BD_SMAContext())
                    {
                        using (var transaction = ctx.Database.BeginTransaction())
                        {
                            try
                            {
                                TUsuario usuario = ctx.TUsuarios.Find(model.IdUsuario);
                                if (usuario != null)
                                {
                                    usuario.Clave = Encrypt.EncrypthAES(model.Password);
                                    usuario.PrimerIngreso = false;
                                    ctx.Entry(usuario).State = EntityState.Modified;
                                    int retorno = ctx.SaveChanges();

                                    if (retorno != 0)
                                    {
                                        TempData["success"] = "Éxito al cambiar contraseña, ingrese sus nuevas credenciales!";
                                        transaction.Commit();
                                    }
                                    else
                                    {
                                        TempData["warn"] = "Hubo un error al cambiar contraseña";
                                    }
                                }
                                else
                                {
                                    TempData["mensaje"] = "El usuario no existe";
                                }
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
                    TempData["mensaje"] = "Hubieron errores de validación";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove("usuario");
            return RedirectToAction("Index");
        }






        [HttpGet("Denied")]
        public IActionResult Denied()
        {
            return View();
        }

        public void AutenticarCookies(TUsuario usuario)
        {
            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                                new Claim("Correo", usuario.CorreoElectronico),
                                new Claim(ClaimTypes.Role, usuario.IdRolNavigation.TipoRol)
                                //new Claim("oUsuario", JsonConvert.SerializeObject(usuario)),
                                //new Claim("Id", usuario.IdUsuario.ToString()),
                                //new Claim("NombreCompleto", usuario.Nombre+" "+usuario.Apellidos),                                
                                //new Claim("DescripcionRol", usuario.IdRolNavigation.TipoRol),                                
                            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
        }
    }
}
