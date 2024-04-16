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
            var proyectoWebAvanzadoContext = _context.Treceta.Include(t => t.Usuario);
            return View(await proyectoWebAvanzadoContext.ToListAsync());
        }

        public async Task<IActionResult> MisRecetas()
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

        // POST: Treceta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Treceta == null)
            {
                return Problem("Entity set 'ProyectoWebAvanzadoContext.Treceta'  is null.");
            }
            var trecetum = await _context.Treceta.FindAsync(id);
            if (trecetum != null)
            {
                _context.Treceta.Remove(trecetum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrecetumExists(long id)
        {
          return (_context.Treceta?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
