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
    public class TcursoRecetumsController : Controller

    {
        private readonly ProyectoWebAvanzadoContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TcursoRecetumsController(ProyectoWebAvanzadoContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TcursoRecetums
        public async Task<IActionResult> Index()
        {
            var proyectoWebAvanzadoContext = _context.TcursoReceta.Include(t => t.Curso).Include(t => t.Receta);
            return View(await proyectoWebAvanzadoContext.ToListAsync());
        }

        // GET: TcursoRecetums/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.TcursoReceta == null)
            {
                return NotFound();
            }

            var tcursoRecetum = await _context.TcursoReceta
                .Include(t => t.Curso)
                .Include(t => t.Receta)
                .FirstOrDefaultAsync(m => m.CursoId == id);
            if (tcursoRecetum == null)
            {
                return NotFound();
            }

            return View(tcursoRecetum);
        }

        // GET: TcursoRecetums/Create
        public async Task<IActionResult> Create()
        {
            // Obtener los IDs de los usuarios con el rol de Profesor
            var profesores = await _userManager.GetUsersInRoleAsync("Profesor");

            // Obtener las recetas creadas por usuarios con el rol de Profesor
            var recetasDeProfesores = _context.Treceta
                .AsEnumerable() // Convertir a enumerable para permitir la comparación en memoria
                .Where(r => profesores.Any(p => p.Id == r.UsuarioId))
                .ToList();

            // Crear una lista de SelectListItem para pasar a la vista
            var recetasSelectList = recetasDeProfesores
            .Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = $"{r.Nombre} - {profesores.FirstOrDefault(p => p.Id == r.UsuarioId)?.Nombre} {profesores.FirstOrDefault(p => p.Id == r.UsuarioId)?.Apellidos}"
            })
            .ToList();

            ViewData["Cursos"] = new SelectList(_context.Tcursos.Include(c => c.Usuario).ToList(), "Id", "CursoConProfesor");
            ViewData["Recetas"] = new SelectList(recetasSelectList, "Value", "Text");
            return View();
        }


        // POST: TcursoRecetums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CursoId,RecetaId")] TcursoRecetum tcursoRecetum)
        {
            
                _context.Add(tcursoRecetum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["CursoId"] = new SelectList(_context.Tcursos, "Id", "Id", tcursoRecetum.CursoId);
            ViewData["RecetaId"] = new SelectList(_context.Treceta, "Id", "Id", tcursoRecetum.RecetaId);
            return View(tcursoRecetum);
        }

        // GET: TcursoRecetums/Edit/5


        // GET: TcursoRecetums/Edit/5
        public async Task<IActionResult> Edit(long? cursoId, long? recetaId)
        {
            if (cursoId == null || recetaId == null)
            {
                return NotFound();
            }

            var tcursoRecetum = await _context.TcursoReceta.FindAsync(cursoId, recetaId);
            if (tcursoRecetum == null)
            {
                return NotFound();
            }
            ViewData["CursoId"] = new SelectList(_context.Tcursos, "Id", "Id", tcursoRecetum.CursoId);
            ViewData["RecetaId"] = new SelectList(_context.Treceta, "Id", "Id", tcursoRecetum.RecetaId);
            return View(tcursoRecetum);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CursoId,RecetaId")] TcursoRecetum tcursoRecetum)
        {
            if (id != tcursoRecetum.CursoId)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(tcursoRecetum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TcursoRecetumExists(tcursoRecetum.CursoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["CursoId"] = new SelectList(_context.Tcursos, "Id", "Id", tcursoRecetum.CursoId);
            ViewData["RecetaId"] = new SelectList(_context.Treceta, "Id", "Id", tcursoRecetum.RecetaId);
            return View(tcursoRecetum);
        }

        // GET: TcursoRecetums/Delete/5
        // GET: TcursoRecetums/Delete
        public async Task<IActionResult> Delete(long? cursoId, long? recetaId)
        {
            if (cursoId == null || recetaId == null)
            {
                return NotFound();
            }

            var tcursoRecetum = await _context.TcursoReceta.FindAsync(cursoId, recetaId);
            if (tcursoRecetum == null)
            {
                return NotFound();
            }

            return View(tcursoRecetum);
        }

        // POST: TcursoRecetums/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long cursoId, long recetaId)
        {
            var tcursoRecetum = await _context.TcursoReceta.FindAsync(cursoId, recetaId);
            if (tcursoRecetum == null)
            {
                return NotFound();
            }

            _context.TcursoReceta.Remove(tcursoRecetum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool TcursoRecetumExists(long id)
        {
            return (_context.TcursoReceta?.Any(e => e.CursoId == id)).GetValueOrDefault();
        }
    }
}
