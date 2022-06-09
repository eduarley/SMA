using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{
    public partial class TPrioridadTicket
    {
        public TPrioridadTicket()
        {
            TRevisionTickets = new HashSet<TRevisionTicket>();
            TTickets = new HashSet<TTicket>();
        }

        public int IdPrioridad { get; set; }
        public string TipoPrioridad { get; set; }

        public virtual ICollection<TRevisionTicket> TRevisionTickets { get; set; }
        public virtual ICollection<TTicket> TTickets { get; set; }
    }
}
