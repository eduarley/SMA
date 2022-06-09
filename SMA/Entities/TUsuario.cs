using Microsoft.AspNetCore.Mvc;
using SMA.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{
    [ModelMetadataType(typeof(UsuarioMetadata))]
    public partial class TUsuario
    {
        public TUsuario()
        {
            TNotificacionGeneralIdEmisorNavigations = new HashSet<TNotificacionGeneral>();
            TNotificacionGeneralIdReceptorNavigations = new HashSet<TNotificacionGeneral>();
            TRevisionTickets = new HashSet<TRevisionTicket>();
            TTickets = new HashSet<TTicket>();
        }

        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string Clave { get; set; }
        public byte[] Foto { get; set; }
        public int IdDepartamento { get; set; }
        public int IdRol { get; set; }
        public string Cedula { get; set; }
        public bool Estado { get; set; }
        public bool PrimerIngreso { get; set; }
        public string Token { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual TDepartamento IdDepartamentoNavigation { get; set; }
        public virtual TRol IdRolNavigation { get; set; }
        public virtual ICollection<TNotificacionGeneral> TNotificacionGeneralIdEmisorNavigations { get; set; }
        public virtual ICollection<TNotificacionGeneral> TNotificacionGeneralIdReceptorNavigations { get; set; }
        public virtual ICollection<TRevisionTicket> TRevisionTickets { get; set; }
        public virtual ICollection<TTicket> TTickets { get; set; }
    }
}
