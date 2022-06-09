using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SMA.Controllers
{
    [Route("ErrorPage/{statuscode}")]
    public class ErrorController : Controller
    {
        public IActionResult Index(int statuscode)
        {
            switch (statuscode)
            {
                case (int)HttpStatusCode.NotFound:
                    ViewData["Codigo"] = statuscode;
                    ViewData["Error"] = "Recurso no encontrado.";
                    break;

                case (int)HttpStatusCode.Unauthorized:
                    ViewData["Codigo"] = statuscode;
                    ViewData["Error"] = "Acceso denegado.";
                    break;

                case (int)HttpStatusCode.Forbidden:
                    ViewData["Codigo"] = statuscode;
                    ViewData["Error"] = "Prohibido.";
                    break;

                default:
                    ViewData["Codigo"] = statuscode;
                    ViewData["Error"] = "Error.";
                    break;
            }
            return View("ErrorPage");
        }
    }
}
