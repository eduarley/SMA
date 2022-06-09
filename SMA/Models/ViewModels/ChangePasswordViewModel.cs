using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMA.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        //Este token solamente se usa al recuperar la clave
        public string Token { get; set; }


        public int IdUsuario { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirmación de contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido")]
        [Compare(nameof(Password), ErrorMessage = "La contraseña debe coincidir en ambos campos")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre {1} y {2} caracteres")]
        public string ConfirmPassword { get; set; }
    }
}
