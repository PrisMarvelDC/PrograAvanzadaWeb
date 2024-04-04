using System;
using System.Collections.Generic;

namespace ProyectoPAW.Models
{
    public partial class TlogErrore
    {
        public long Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Usuario { get; set; }
        public string? Modulo { get; set; }
        public string DescripcionError { get; set; } = null!;
        public string? InformacionAdicional { get; set; }
    }
}
