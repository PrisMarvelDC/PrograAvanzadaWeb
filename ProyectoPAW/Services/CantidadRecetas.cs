using ProyectoPAW.Models;

namespace ProyectoPAW.Services
{
    public class CantidadRecetas : ICantidadRecetas
    {
        private readonly ProyectoWebAvanzadoContext _context;
        public CantidadRecetas(ProyectoWebAvanzadoContext context)
        {
            _context = context;
        }

        public int ObtenerCantidadRecetas()
        {
            try
            {
                var consulta = _context.TcursoReceta.Count();
                return consulta;
            }
            catch
            {
                return 0;
            }
        }

        public int ObtenerCantidadRecetas(int IDCurso)
        {
            try
            {
                var consulta = _context.TcursoReceta.Where(x => x.CursoId == IDCurso);
                if (consulta.Any())
                    return consulta.Count();
                else
                    return 0;

            }
            catch
            {
                return 0;
            }
        }

       
    }
}
