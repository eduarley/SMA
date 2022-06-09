using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMA.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using SMA.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace SMA.Controllers
{
    [Authorize(Roles = "ADMIN,REGULAR")]
    public class NotificacionController : Controller
    {

        private List<NotificacionViewModel> _oNotifications = new List<NotificacionViewModel>();


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetNotifications()
        {
            try
            {
                //Importante tener autenticado porque sino, se cae al inicio
                if (User.Identity.IsAuthenticated)
                {
                    TUsuario usuario = JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario"));
                    List<TNotificacionGeneral> generales = new List<TNotificacionGeneral>();

                    if (usuario != null)
                    {
                        using (BD_SMAContext ctx = new BD_SMAContext())
                        {
                            if (string.Equals(usuario.IdRolNavigation.TipoRol, "admin", StringComparison.OrdinalIgnoreCase))
                            {
                                generales = await ctx
                               .TNotificacionGenerals
                               .OrderByDescending(x => x.Fecha)
                               .Where(x => x.IdReceptor == null)
                               //.Include(x => x.IdEmisorNavigation)
                               .ToListAsync();
                                foreach (var item in generales)
                                {
                                    _oNotifications.Add(new NotificacionViewModel()
                                    {
                                        IdNotificacion = item.IdNotificacion,
                                        NumTicket = item.NumTicket,
                                        IdEmisor = item.IdEmisor,
                                        Titulo = item.Titulo,
                                        Descripcion = item.Descripcion,
                                        //Fecha = item.Fecha.ToString("dd/MM/yyyy hh:mm tt"),
                                        Fecha = SubstracDate(item.Fecha),
                                        Visto = item.Visto,
                                    });
                                }
                            }
                            else
                            {
                                generales = await ctx
                               .TNotificacionGenerals
                               .OrderByDescending(x => x.Fecha)
                               .Where(x => x.IdReceptor == usuario.IdUsuario)
                               .ToListAsync();
                                foreach (var item in generales)
                                {
                                    _oNotifications.Add(new NotificacionViewModel()
                                    {
                                        IdNotificacion = item.IdNotificacion,
                                        NumTicket = item.NumTicket,
                                        IdEmisor = item.IdEmisor,
                                        Titulo = item.Titulo,
                                        Descripcion = item.Descripcion,
                                        IdReceptor = (int)item.IdReceptor,
                                        Fecha = SubstracDate(item.Fecha),
                                        Visto = item.Visto,
                                    });
                                }
                            }

                        }
                    }
                }

                return Json(_oNotifications);
            }
            catch (Exception ex)
            {

                return Json(ex);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {

                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    TNotificacionGeneral notificacion = await ctx.TNotificacionGenerals
                        .Where(x => x.IdNotificacion == id)
                        .Include(x => x.IdEmisorNavigation)
                        .FirstOrDefaultAsync();

                    if (notificacion == null)
                    {
                        TempData["warn"] = "No existe el registro seleccionado";
                        return RedirectToAction("Index");
                    }


                    notificacion.Visto = true;
                    ctx.Entry(notificacion).State = EntityState.Modified;
                    await ctx.SaveChangesAsync();
                    return View(notificacion);
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("Index");
            }


        }





        [HttpGet]
        public async Task<IActionResult> AllNotifications()
        {
            try
            {
                try
                {

                    using (BD_SMAContext ctx = new BD_SMAContext())
                    {
                        TUsuario usuario = JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario"));
                        List<TNotificacionGeneral> notificaciones = new List<TNotificacionGeneral>();

                        if (string.Equals(usuario.IdRolNavigation.TipoRol, "admin", StringComparison.OrdinalIgnoreCase))
                        {
                            notificaciones = await ctx.TNotificacionGenerals
                            .OrderByDescending(x => x.Fecha)
                            .Where(x => x.IdReceptor == null)
                            .Include(x => x.IdEmisorNavigation)
                            .ToListAsync();
                        }
                        else
                        {
                            notificaciones = await ctx.TNotificacionGenerals
                           .OrderByDescending(x => x.Fecha)
                           .Where(x => x.IdReceptor == usuario.IdUsuario)
                           .Include(x => x.IdEmisorNavigation)
                           .ToListAsync();
                        }



                        if (notificaciones == null)
                        {
                            TempData["warn"] = "No existen notificaciones relacionadas.";
                            return RedirectToAction("Index");
                        }

                        return View(notificaciones);
                    }

                }
                catch (Exception ex)
                {
                    TempData["error"] = "Error fatal: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    string script = "DELETE FROM T_NotificacionGeneral WHERE id_notificacion = " + id;

                   await ctx.Database.ExecuteSqlRawAsync(script);
                    TempData["success"] = "Notificación eliminada con éxito";
                    return RedirectToAction("AllNotifications");
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
                return RedirectToAction("AllNotifications");
            }
        }

       /* [HttpGet] //AJAX
        public void DeleteAll(int id)
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    string script = "DELETE FROM T_NotificacionGeneral";

                    ctx.Database.ExecuteSqlRaw(script);
                    TempData["success"] = "Notificaciones eliminadas con éxito";
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error fatal: " + ex.Message;
            }
            
        }*/

        private string SubstracDate(DateTime fecha)
        {

            DateTime actual = DateTime.Now;
            var resultado = Convert.ToInt32((actual - fecha).TotalHours);
            string tiempo = " horas.";

            if (resultado < 1)
            {
                resultado = Convert.ToInt32((actual - fecha).TotalMinutes);
                if (resultado == 1)
                    tiempo = " minuto.";
                else
                    tiempo = " minutos.";
            }
            else
            {
                tiempo = " hora.";
            }


            if (resultado >= 24)
            {
                resultado = Convert.ToInt32((actual - fecha).TotalDays);
                if (resultado == 1)
                    tiempo = " día.";
                else
                    tiempo = " días.";
            }


            return Convert.ToString("Hace " + resultado + tiempo);
        }
    }
}
