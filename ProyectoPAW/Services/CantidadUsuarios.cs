using ProyectoPAW.Models;

namespace ProyectoPAW.Services
{
    public class CantidadUsuarios : ICantidadUsuarios
    {
        private readonly ProyectoWebAvanzadoContext _context;
        public CantidadUsuarios(ProyectoWebAvanzadoContext context)
        {
            _context = context;
        }

        public int ObtenerCantidadUsuarios()
        {
            try
            {
                var consulta = _context.TcursoUsuarios.Count();
                return consulta;
            }
            catch
            {
                return 0;
            }
        }

        public int ObtenerCantidadUsuarios(int IDCurso)
        {
            try
            {
                var consulta = _context.TcursoUsuarios.Where(x => x.CursoId == IDCurso);
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
