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
    public class TcursoController : Controller
    {
        private readonly ProyectoWebAvanzadoContext _context;

        public TcursoController(ProyectoWebAvanzadoContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
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
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", tcurso.UsuarioId);
            return View(tcurso);
        }

        // GET: Tcurso/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Tcursos == null)
            {
                return NotFound();
            }

            var tcurso = await _context.Tcursos.FindAsync(id);
            if (tcurso == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", tcurso.UsuarioId);
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
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", tcurso.UsuarioId);
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
            if (_context.Tcursos == null)
            {
                return Problem("Entity set 'ProyectoWebAvanzadoContext.Tcursos'  is null.");
            }
            var tcurso = await _context.Tcursos.FindAsync(id);
            if (tcurso != null)
            {
                _context.Tcursos.Remove(tcurso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TcursoExists(long id)
        {
          return (_context.Tcursos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
