using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{
    public partial class TRol
    {
        public TRol()
        {
            TUsuarios = new HashSet<TUsuario>();
        }

        public int IdRol { get; set; }
        public string TipoRol { get; set; }

        public virtual ICollection<TUsuario> TUsuarios { get; set; }
    }
}
