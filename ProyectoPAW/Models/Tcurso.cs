using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage ="Este dato es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        [StringLength(200, ErrorMessage = "Los comentarios no pueden tener más de 200 caracteres.")]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public string CursoConProfesor => $"{Nombre} - Profesor: {Usuario.Nombre} {Usuario.Apellidos}";

        public string Profesor { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        [DisplayName("Profesor")]
        public string UsuarioId { get; set; }

        [DisplayName("Profesor")]
        public virtual AspNetUser? Usuario { get; set; } 
        public virtual ICollection<TcursoRecetum>? TcursoReceta { get; set; }
        public virtual ICollection<TcursoUsuario>? TcursoUsuarios { get; set; }
    }
}
