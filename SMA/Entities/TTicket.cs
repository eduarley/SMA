using Microsoft.AspNetCore.Mvc;
using SMA.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{
    [ModelMetadataType(typeof(TicketMetadata))]
    public partial class TTicket
    {
        public TTicket()
        {
            TArchivos = new HashSet<TArchivo>();
            TComprobantes = new HashSet<TComprobante>();
            TNotificacionGenerals = new HashSet<TNotificacionGeneral>();
        }

        public int NumTicket { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public int IdPrioridad { get; set; }
        public int? IdRevision { get; set; }
        public bool Revisado { get; set; }

        public virtual TPrioridadTicket IdPrioridadNavigation { get; set; }
        public virtual TRevisionTicket IdRevisionNavigation { get; set; }
        public virtual TUsuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<TArchivo> TArchivos { get; set; }
        public virtual ICollection<TComprobante> TComprobantes { get; set; }
        public virtual ICollection<TNotificacionGeneral> TNotificacionGenerals { get; set; }
    }
}
