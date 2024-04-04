using System;
using System.Collections.Generic;

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
        public string Nombre { get; set; } 
        public string Descripcion { get; set; } 
        public string Instrucciones { get; set; } 
        public string Categoria { get; set; } 
        public string Ingredientes { get; set; } 

        public virtual AspNetUser? Usuario { get; set; } 
        public virtual ICollection<TcursoRecetum>? TcursoReceta { get; set; }
    }
}
