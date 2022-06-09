using SMA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMA.Models.ViewModels
{
    public class RecoveryPasswordViewModel
    {
        public TUsuario Usuario { get; set; }

        public string Token { get; set; }

        [Display(Name = "Cédula")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido")]
        public string Cedula { get; set; }


        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo electrónico")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido")]
        public string Correo { get; set; }
    }
}
