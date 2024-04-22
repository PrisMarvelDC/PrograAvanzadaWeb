using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPAW.Models;

namespace ProyectoPAW.Controllers
{
    public class TcursoUsuariosController : Controller
    {
        private readonly ProyectoWebAvanzadoContext _context;

        public TcursoUsuariosController(ProyectoWebAvanzadoContext context)
        {
            _context = context;
        }

        // GET: TcursoUsuarios
        public async Task<IActionResult> Index()
        {
            var proyectoWebAvanzadoContext = _context.TcursoUsuarios.Include(t => t.Curso).Include(t => t.Usuario);
            return View(await proyectoWebAvanzadoContext.ToListAsync());
        }

        // GET: TcursoUsuarios/Details/5
        public async Task<IActionResult> Details(long? cursoId, long? usuarioID )
        {
            if (cursoId == null || _context.TcursoUsuarios == null)
            {
                return NotFound();
            }

            var tcursoUsuario = await _context.TcursoUsuarios
                .Include(t => t.Curso)
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.CursoId == cursoId);
            if (tcursoUsuario == null)
            {
                return NotFound();
            }

            return View(tcursoUsuario);
        }

        // GET: TcursoUsuarios/Create
        public IActionResult Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Tcursos.Include(c => c.Usuario), "Id", "CursoConProfesor");
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Nombre");
            return View();
        }

        // POST: TcursoUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CursoId,UsuarioId")] TcursoUsuario tcursoUsuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tcursoUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CursoId"] = new SelectList(_context.Tcursos, "Id", "CursoConProfesor", tcursoUsuario.CursoId);
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "NombreCompleto", tcursoUsuario.UsuarioId);
            return View(tcursoUsuario);
        }

        // GET: TcursoUsuarios/Matricula
        public IActionResult Matricula()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obtener el ID del usuario logueado
            var cursos = _context.Tcursos.ToList();
            ViewData["CursoId"] = new SelectList(cursos, "Id", "Nombre");
            ViewData["UsuarioId"] = userId; // Establecer el ID de usuario en la vista
            return View();
        }

        // POST: TcursoUsuarios/Matricula
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Matricula([Bind("CursoId,UsuarioId")] TcursoUsuario tcursoUsuario)
        {
            // Obtener el ID del usuario logueado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verificar si el usuario ya está matriculado en el curso
            var existingMatricula = await _context.TcursoUsuarios
                .FirstOrDefaultAsync(m => m.CursoId == tcursoUsuario.CursoId && m.UsuarioId == userId);

            if (existingMatricula != null)
            {
                // El usuario ya está matriculado en el curso, redirigir a Index
                return RedirectToAction("Index", "Tcurso");
            }

            // Asignar el ID de usuario al modelo TcursoUsuario
            tcursoUsuario.UsuarioId = userId;

            _context.Add(tcursoUsuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Tcurso");
        }



        // GET: TcursoUsuarios/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.TcursoUsuarios == null)
            {
                return NotFound();
            }

            var tcursoUsuario = await _context.TcursoUsuarios.FindAsync(id);
            if (tcursoUsuario == null)
            {
                return NotFound();
            }
            ViewData["CursoId"] = new SelectList(_context.Tcursos, "Id", "Id", tcursoUsuario.CursoId);
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", tcursoUsuario.UsuarioId);
            return View(tcursoUsuario);
        }

        // POST: TcursoUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CursoId,UsuarioId")] TcursoUsuario tcursoUsuario)
        {
            if (id != tcursoUsuario.CursoId)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(tcursoUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TcursoUsuarioExists(tcursoUsuario.CursoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["CursoId"] = new SelectList(_context.Tcursos, "Id", "Id", tcursoUsuario.CursoId);
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", tcursoUsuario.UsuarioId);
            return View(tcursoUsuario);
        }

        // GET: TcursoUsuarios/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.TcursoUsuarios == null)
            {
                return NotFound();
            }

            var tcursoUsuario = await _context.TcursoUsuarios
                .Include(t => t.Curso)
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.CursoId == id);
            if (tcursoUsuario == null)
            {
                return NotFound();
            }

            return View(tcursoUsuario);
        }

        // POST: TcursoUsuarios/Delete/5


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long cursoId, string usuarioId)
        {
            var tcursoUsuario = await _context.TcursoUsuarios.FirstOrDefaultAsync(m => m.CursoId == cursoId && m.UsuarioId == usuarioId);
            if (tcursoUsuario == null)
            {
                return NotFound();
            }

            _context.TcursoUsuarios.Remove(tcursoUsuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: TcursoUsuarios/QuitarMatricula
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuitarMatricula(long cursoId, string usuarioId)
        {
            var tcursoUsuario = await _context.TcursoUsuarios.FirstOrDefaultAsync(m => m.CursoId == cursoId && m.UsuarioId == usuarioId);
            if (tcursoUsuario == null)
            {
                return NotFound();
            }

            _context.TcursoUsuarios.Remove(tcursoUsuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool TcursoUsuarioExists(long id)
        {
          return (_context.TcursoUsuarios?.Any(e => e.CursoId == id)).GetValueOrDefault();
        }
    }
}
