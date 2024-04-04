using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoPAW.Controllers
{
    public class RoleController : Controller
    {
        [Authorize(Policy = "RequireProfesor")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        //[Authorize(Roles = "Administrador,Profesor")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Policy = "RequireEstudiante")]
        public IActionResult Estudiante()
        {
            return View();
        }
    }
}
