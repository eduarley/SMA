using Microsoft.AspNetCore.Mvc;
using SMA.Common;
using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{
    [ModelMetadataType(typeof(NotificacionMetadata))]
    public partial class TNotificacionGeneral
    {
        public int IdNotificacion { get; set; }
        public int NumTicket { get; set; }
        public int IdEmisor { get; set; }
        public int? IdReceptor { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public bool Visto { get; set; }

        public virtual TUsuario IdEmisorNavigation { get; set; }
        public virtual TUsuario IdReceptorNavigation { get; set; }
        public virtual TTicket NumTicketNavigation { get; set; }
    }
}
