using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewData["CursoId"] = new SelectList(_context.Tcursos, "Id", "Id");
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
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
            ViewData["CursoId"] = new SelectList(_context.Tcursos, "Id", "Id", tcursoUsuario.CursoId);
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", tcursoUsuario.UsuarioId);
            return View(tcursoUsuario);
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

            if (ModelState.IsValid)
            {
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
            }
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
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.TcursoUsuarios == null)
            {
                return Problem("Entity set 'ProyectoWebAvanzadoContext.TcursoUsuarios'  is null.");
            }
            var tcursoUsuario = await _context.TcursoUsuarios.FindAsync(id);
            if (tcursoUsuario != null)
            {
                _context.TcursoUsuarios.Remove(tcursoUsuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TcursoUsuarioExists(long id)
        {
          return (_context.TcursoUsuarios?.Any(e => e.CursoId == id)).GetValueOrDefault();
        }
    }
}
