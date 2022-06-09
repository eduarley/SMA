using Microsoft.AspNetCore.Mvc;
using SMA.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{
    [ModelMetadataType(typeof(RevisionMetadata))]
    public partial class TRevisionTicket
    {
        public TRevisionTicket()
        {
            TTickets = new HashSet<TTicket>();
        }

        public int IdRevision { get; set; }
        public int IdUsuario { get; set; }
        public int IdPrioridad { get; set; }
        public int IdEstado { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaRevision { get; set; }

        public virtual TEstadoAceptacion IdEstadoNavigation { get; set; }
        public virtual TPrioridadTicket IdPrioridadNavigation { get; set; }
        public virtual TUsuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<TTicket> TTickets { get; set; }
    }
}
