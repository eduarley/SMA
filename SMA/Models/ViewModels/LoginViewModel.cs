using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMA.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required( ErrorMessage ="{0} es un campo requerido")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Clave { get; set; }
    }
}
