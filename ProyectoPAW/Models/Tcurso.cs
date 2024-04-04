using System;
using System.Collections.Generic;

namespace ProyectoPAW.Models
{
    public partial class Tcurso
    {
        public Tcurso()
        {
            TcursoReceta = new HashSet<TcursoRecetum>();
            TcursoUsuarios = new HashSet<TcursoUsuario>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; } 
        public string Descripcion { get; set; } 
        public string Profesor { get; set; } 
        public string UsuarioId { get; set; } 

        public virtual AspNetUser? Usuario { get; set; } 
        public virtual ICollection<TcursoRecetum>? TcursoReceta { get; set; }
        public virtual ICollection<TcursoUsuario>? TcursoUsuarios { get; set; }
    }
}
