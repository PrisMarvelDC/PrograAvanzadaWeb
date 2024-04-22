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
    public class TrecetaController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ProyectoWebAvanzadoContext _context;

        public TrecetaController(UserManager<ApplicationUser> userManager, ProyectoWebAvanzadoContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Treceta
        public async Task<IActionResult> Index()
        {
            // Obtener los IDs de los usuarios con el rol de Profesor
            var profesores = await _userManager.GetUsersInRoleAsync("Profesor");

            // Obtener las recetas creadas por usuarios con el rol de Profesor
            var recetasDeProfesores = _context.Treceta
                .Include(t => t.Usuario)
                .ToList() // Convertir a lista para permitir la comparación en memoria
                .Where(r => profesores.Any(p => p.Id == r.UsuarioId));

            return View(recetasDeProfesores);
        }

        public async Task<IActionResult> MisRecetas()
        {
            var proyectoWebAvanzadoContext = _context.Treceta.Include(t => t.Usuario);
            return View(await proyectoWebAvanzadoContext.ToListAsync());
        }

        public async Task<IActionResult> RecetasProfesor()
        {
            var proyectoWebAvanzadoContext = _context.Treceta.Include(t => t.Usuario);
            return View(await proyectoWebAvanzadoContext.ToListAsync());
        }

        // GET: Treceta/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Treceta == null)
            {
                return NotFound();
            }

            var trecetum = await _context.Treceta
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trecetum == null)
            {
                return NotFound();
            }

            return View(trecetum);
        }

        // GET: Treceta/Create
        public IActionResult Create()
        {
            //ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Treceta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,Nombre,Descripcion,Instrucciones,Categoria,Ingredientes")] Treceta trecetum)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                trecetum.UsuarioId = user.Id;

                _context.Add(trecetum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", trecetum.UsuarioId);
            return View(trecetum);
        }

        // GET: Treceta/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Treceta == null)
            {
                return NotFound();
            }

            var trecetum = await _context.Treceta.FindAsync(id);
            if (trecetum == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", trecetum.UsuarioId);
            return View(trecetum);
        }

        // POST: Treceta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UsuarioId,Nombre,Descripcion,Instrucciones,Categoria,Ingredientes")] Treceta trecetum)
        {
            if (id != trecetum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    trecetum.UsuarioId = user.Id;
                    _context.Update(trecetum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrecetumExists(trecetum.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", trecetum.UsuarioId);
            return View(trecetum);
        }

        // GET: Treceta/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Treceta == null)
            {
                return NotFound();
            }

            var trecetum = await _context.Treceta
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trecetum == null)
            {
                return NotFound();
            }

            return View(trecetum);
        }

        // POST: Trecetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var treceta = await _context.Treceta.FindAsync(id);
            if (treceta == null)
            {
                return NotFound();
            }

            // Eliminar todas las relaciones TcursoRecetum relacionadas
            var relaciones = _context.TcursoReceta.Where(r => r.RecetaId == id);
            _context.TcursoReceta.RemoveRange(relaciones);

            // Ahora elimina la receta
            _context.Treceta.Remove(treceta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool TrecetumExists(long id)
        {
          return (_context.Treceta?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
