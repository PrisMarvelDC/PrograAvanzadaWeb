using System;
using System.Collections.Generic;

namespace ProyectoPAW.Models
{
    public partial class TcursoUsuario
    {
        public long Id { get; set; }
        public long CursoId { get; set; }
        public string UsuarioId { get; set; } = null!;

        public virtual Tcurso? Curso { get; set; } = null!;
        public virtual AspNetUser? Usuario { get; set; } = null!;
    }
}
