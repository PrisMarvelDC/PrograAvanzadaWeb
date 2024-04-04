using System;
using System.Collections.Generic;

namespace ProyectoPAW.Models
{
    public partial class TcursoRecetum
    {
        public long Id { get; set; }
        public long CursoId { get; set; }
        public long RecetaId { get; set; }

        public virtual Tcurso Curso { get; set; } = null!;
        public virtual Treceta Receta { get; set; } = null!;
    }
}
