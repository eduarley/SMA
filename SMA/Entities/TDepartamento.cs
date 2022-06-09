using System;
using System.Collections.Generic;

#nullable disable

namespace SMA.Entities
{
    public partial class TDepartamento
    {
        public TDepartamento()
        {
            TUsuarios = new HashSet<TUsuario>();
        }

        public int IdDepartamento { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<TUsuario> TUsuarios { get; set; }
    }
}
