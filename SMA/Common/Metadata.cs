using SMA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMA.Common
{
    internal partial class UsuarioMetadata
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        public int IdUsuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        [StringLength(30, MinimumLength = 0, ErrorMessage = "{0} debe ser menor a {1} caracteres")]
        public string Nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{0} debe ser menor a {1} caracteres")]
        public string Apellidos { get; set; }

        [Display(Name = "Correo")]
        [RegularExpression(@"([a-zA-Z]+(\.[a-zA-Z]+)+)@munisarchi\.go\.cr", ErrorMessage = "Formato inválido. Ej: nombre.apellido@munisarchi.go.cr")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "{0} debe ser menor a {1} caracteres")]
        public string CorreoElectronico { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        //[StringLength(25, MinimumLength = 6, ErrorMessage = "{0} debe ser entre {2} y {1} caracteres.")]
        public string Clave { get; set; }

        [Display(Name = "Departamento")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        public int IdDepartamento { get; set; }

        [Display(Name = "Rol")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        public int IdRol { get; set; }

        [Display(Name = "Cédula")]
        [RegularExpression(@"[0-9]+",
         ErrorMessage = "Únicamente números. Ej: 101230456")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        [StringLength(15, MinimumLength = 9, ErrorMessage = "{0} debe ser entre {2} y {1} caracteres.")]
        public string Cedula { get; set; }

        public bool Estado { get; set; }
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion { get; set; }


        [Display(Name = "Departamento")]
        public virtual TDepartamento IdDepartamentoNavigation { get; set; }
        [Display(Name = "Rol")]
        public virtual TRol IdRolNavigation { get; set; }
    }



    internal partial class TicketMetadata
    {
        [Display(Name = "Título")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        [StringLength(200, MinimumLength = 0, ErrorMessage = "{0} debe ser menor a {1} caracteres")]
        public string Titulo { get; set; }
        [Display(Name = "Descripción")]
        [StringLength(600, MinimumLength = 0, ErrorMessage = "{0} debe ser menor a {1} caracteres")]
        public string Descripcion { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm tt}")]
        public DateTime Fecha { get; set; }
        [Display(Name = "Prioridad")]
        public virtual TPrioridadTicket IdPrioridadNavigation { get; set; }

        [Display(Name = "Prioridad sugerida")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        public int IdPrioridad { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        public int IdUsuario { get; set; }
    }

    internal partial class NotificacionMetadata
    {
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime Fecha { get; set; }

        public string FechaStr => this.Fecha.ToString("dd-MM-yyyy HH:mm");

        [Display(Name = "Tiket N°")]
        public int NumTicket { get; set; }

        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
    }


    internal partial class RevisionMetadata
    {
        [StringLength(600, MinimumLength = 0, ErrorMessage = "{0} debe ser menor a {1} caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} es un campo requerido.")]
        public string Observacion { get; set; }
    }


}
