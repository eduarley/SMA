using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{
    public partial class TEstadoAceptacion
    {
        public TEstadoAceptacion()
        {
            TRevisionTickets = new HashSet<TRevisionTicket>();
        }

        public int IdEstado { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<TRevisionTicket> TRevisionTickets { get; set; }
    }
}
