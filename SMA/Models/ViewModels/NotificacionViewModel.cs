using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMA.Models.ViewModels
{
    public class NotificacionViewModel
    {
        //General
        public int IdNotificacion { get; set; }
        public int NumTicket { get; set; }
        public int IdEmisor { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
        public bool Visto { get; set; }


        //Revision
        public int IdReceptor { get; set; }
        public int IdRevision { get; set; }
    }
}
