using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public string CursoConProfesor => $"{Nombre} - Profesor: {Usuario.Nombre} {Usuario.Apellidos}";

        public string Profesor { get; set; }

        [DisplayName("Profesor")]
        public string UsuarioId { get; set; }

        [DisplayName("Profesor")]
        public virtual AspNetUser? Usuario { get; set; } 
        public virtual ICollection<TcursoRecetum>? TcursoReceta { get; set; }
        public virtual ICollection<TcursoUsuario>? TcursoUsuarios { get; set; }
    }
}
