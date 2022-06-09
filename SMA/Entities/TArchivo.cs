using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{

    public partial class TArchivo
    {
        public int IdArchivo { get; set; }
        public int NumTicket { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public byte[] Archivo { get; set; }

        public virtual TTicket NumTicketNavigation { get; set; }
    }
}
