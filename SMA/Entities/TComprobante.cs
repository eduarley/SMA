using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{
    public partial class TComprobante
    {
        public int IdComprobante { get; set; }
        public int NumTicket { get; set; }
        public string EstadoFinal { get; set; }
        public DateTime Fecha { get; set; }
        public string Retroalimentacion { get; set; }
        public string Justificacion { get; set; }

        public virtual TTicket NumTicketNavigation { get; set; }
    }
}
