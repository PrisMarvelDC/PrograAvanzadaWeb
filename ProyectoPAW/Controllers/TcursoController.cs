using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPAW.Areas.Identity.Data;
using ProyectoPAW.Models;

namespace ProyectoPAW.Controllers
{
    public class TcursoController : Controller
    {
        private readonly ProyectoWebAvanzadoContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TcursoController(ProyectoWebAvanzadoContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tcurso
        public async Task<IActionResult> Index()
        {
            var proyectoWebAvanzadoContext = _context.Tcursos.Include(t => t.Usuario);
            return View(await proyectoWebAvanzadoContext.ToListAsync());
        }

        // GET: Tcurso/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Tcursos == null)
            {
                return NotFound();
            }

            var tcurso = await _context.Tcursos
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tcurso == null)
            {
                return NotFound();
            }

            return View(tcurso);
        }

        // GET: Tcurso/Create
        public async Task<IActionResult> Create()
        {
            // Obtener la lista de usuarios con el rol de Profesor
            var profesores = await _userManager.GetUsersInRoleAsync("Profesor");

            // Crear una lista de SelectListItem para pasar a la vista
            var profesoresSelectList = profesores
                .Select(u => new SelectListItem { Value = u.Id, Text = $"{u.Nombre} {u.Apellidos}" })
                .ToList();

            ViewBag.UsuarioId = new SelectList(profesoresSelectList, "Value", "Text");
            return View();
        }




        // POST: Tcurso/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Profesor,UsuarioId")] Tcurso tcurso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tcurso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            // Obtener la lista de usuarios con el rol de Profesor
            var profesores = await _userManager.GetUsersInRoleAsync("Profesor");

            // Crear una lista de SelectListItem para pasar a la vista
            var profesoresSelectList = profesores
                .Select(u => new SelectListItem { Value = u.Id, Text = $"{u.Nombre} {u.Apellidos}" })
                .ToList();

            ViewData["UsuarioId"] = new SelectList(profesoresSelectList, "Value", "Text");
            return View(tcurso);
        }

        // GET: Tcurso/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcurso = await _context.Tcursos.FindAsync(id);
            if (tcurso == null)
            {
                return NotFound();
            }

            // Obtener la lista de usuarios con el rol de Profesor
            var profesores = await _userManager.GetUsersInRoleAsync("Profesor");

            // Crear una lista de SelectListItem para pasar a la vista
            var profesoresSelectList = profesores
                .Select(u => new SelectListItem { Value = u.Id, Text = $"{u.Nombre} {u.Apellidos}" })
                .ToList();

            ViewData["UsuarioId"] = new SelectList(profesoresSelectList, "Value", "Text", tcurso.UsuarioId);
            return View(tcurso);
        }

        // POST: Tcurso/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Nombre,Descripcion,Profesor,UsuarioId")] Tcurso tcurso)
        {
            if (id != tcurso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tcurso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TcursoExists(tcurso.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var profesores = await _userManager.GetUsersInRoleAsync("Profesor");

            // Crear una lista de SelectListItem para pasar a la vista
            var profesoresSelectList = profesores
                .Select(u => new SelectListItem { Value = u.Id, Text = $"{u.Nombre} {u.Apellidos}" })
                .ToList();

            ViewData["UsuarioId"] = new SelectList(profesoresSelectList, "Value", "Text", tcurso.UsuarioId);
            return View(tcurso);
        }

        // GET: Tcurso/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Tcursos == null)
            {
                return NotFound();
            }

            var tcurso = await _context.Tcursos
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tcurso == null)
            {
                return NotFound();
            }

            return View(tcurso);
        }

        // POST: Tcurso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var tcurso = await _context.Tcursos.FindAsync(id);
            if (tcurso == null)
            {
                return NotFound();
            }

            // Eliminar todas las relaciones TcursoRecetum relacionadas
            var relaciones = _context.TcursoReceta.Where(r => r.CursoId == id);
            _context.TcursoReceta.RemoveRange(relaciones);

            // Ahora elimina el curso
            _context.Tcursos.Remove(tcurso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> CurosEstudiantes()
        {
            // Obtener el usuario actualmente autenticado
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Obtener los cursos en los que está matriculado el usuario actual
            var cursosDelUsuario = await _context.TcursoUsuarios
                .Where(cu => cu.UsuarioId == user.Id)
                .Select(cu => cu.Curso)
                .ToListAsync();

            return View(cursosDelUsuario);
        }


        public async Task<IActionResult> DetailsCurosEstudiantes(long? id)
        {
            if (id == null || _context.Tcursos == null)
            {
                return NotFound();
            }

            var tcurso = await _context.Tcursos
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tcurso == null)
            {
                return NotFound();
            }

            return View(tcurso);
        }

        public async Task<IActionResult> CursosProfesor()
        {
            var proyectoWebAvanzadoContext = _context.Tcursos.Include(t => t.Usuario);
            return View(await proyectoWebAvanzadoContext.ToListAsync());
        }

        private bool TcursoExists(long id)
        {
          return (_context.Tcursos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
