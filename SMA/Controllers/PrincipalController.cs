using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SMA.Entities;
using SMA.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMA.Controllers
{
    [Authorize(Roles = "ADMIN,REGULAR")]
    public class PrincipalController : Controller
    {
        public IActionResult Index()
        {
            TUsuario usuario = null;
            if (HttpContext.Session.GetString("usuario") != null)
            {
                usuario = JsonConvert.DeserializeObject<TUsuario>(HttpContext.Session.GetString("usuario"));
                if (string.Equals(usuario.IdRolNavigation.TipoRol, "admin", StringComparison.OrdinalIgnoreCase))
                {
                    return View("dashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Ticket");
                }
            }
            else
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("usuario");
                return RedirectToAction("Index", "Login");
            }

            
        }

        public async Task<JsonResult> GetEstadosChartJSON()
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    List<TTicket> tickets = await
                        ctx
                        .TTickets
                        .ToListAsync();

                    List<ChartEstadoViewModel> lista = new List<ChartEstadoViewModel>();
                    foreach (var item in tickets)
                    {
                        if (item.Revisado)
                        {
                            lista.Add(new ChartEstadoViewModel()
                            {
                                Estado = "Revisado"
                            });
                        }
                        else
                        {
                            lista.Add(new ChartEstadoViewModel()
                            {
                                Estado = "Sin revisar"
                            });
                        }

                    }

                    var result = from item in lista
                                 group item by item.Estado into g
                                 select new ChartEstadoViewModel()
                                 {
                                     Estado = g.Key,
                                     Total = g.Count(),
                                 };
                    return Json(new { JSONList = result });
                }
                

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<JsonResult> GetPrioridadesChartJSON()
        {
            try
            {
                using (BD_SMAContext ctx = new BD_SMAContext())
                {
                    

                    List<TTicket> tickets = await
                        ctx
                        .TTickets
                        .Include(x => x.IdRevisionNavigation.IdPrioridadNavigation)
                        .ToListAsync();


                    List<ChartPrioridadViewModel> lista = new List<ChartPrioridadViewModel>();
                    foreach (var item in tickets)
                    {
                        if(item.IdRevisionNavigation != null)
                        {
                            lista.Add(new ChartPrioridadViewModel()
                            {
                                Prioridad = item.IdRevisionNavigation.IdPrioridadNavigation.TipoPrioridad,
                            });
                        }   
                    }

                    var result = from item in lista
                                 group item by item.Prioridad into g
                                 select new ChartPrioridadViewModel()
                                 {
                                     Prioridad = g.Key,
                                     Total = g.Count(),
                                 };


                    return Json(new { JSONList = result });
                }


            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
