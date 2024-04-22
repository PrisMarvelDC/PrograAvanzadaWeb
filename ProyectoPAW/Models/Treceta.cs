using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPAW.Models
{
    public partial class Treceta
    {
        public Treceta()
        {
            TcursoReceta = new HashSet<TcursoRecetum>();
        }

        public long Id { get; set; }
        
        public string? UsuarioId { get; set; }
        [Required(ErrorMessage = "Este dato es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        [StringLength(200, ErrorMessage = "Los comentarios no pueden tener más de 200 caracteres.")]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        [StringLength(1000, ErrorMessage = "Los comentarios no pueden tener más de 1000 caracteres.")]
        public string Instrucciones { get; set; }
        [Required(ErrorMessage = "Este dato es requerido")]
        [DisplayName("Categoría")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "Este dato es requerido")]
        [StringLength(500, ErrorMessage = "Los comentarios no pueden tener más de 500 caracteres.")]
        public string Ingredientes { get; set; }

        public string RecetaConUsuario => Usuario != null ? $"{Nombre} - Usuario: {Usuario.Nombre} {Usuario.Apellidos}" : Nombre;


        public virtual AspNetUser? Usuario { get; set; } 
        public virtual ICollection<TcursoRecetum>? TcursoReceta { get; set; }
    }
}
