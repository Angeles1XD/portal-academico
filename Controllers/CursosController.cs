using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAcademico.Data;
using PortalAcademico.Models;

namespace PortalAcademico.Controllers
{
    public class CursosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================
        // LISTA
        // ======================
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cursos.ToListAsync());
        }

        // ======================
        // CREATE GET
        // ======================
        public IActionResult Create()
        {
            return View();
        }

        // ======================
        // CREATE POST
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        // ======================
        // EDIT GET
        // ======================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();

            return View(curso);
        }

        // ======================
        // EDIT POST
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Curso curso)
        {
            if (id != curso.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Cursos.Any(e => e.Id == curso.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        // ======================
        // DELETE GET
        // ======================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (curso == null) return NotFound();

            return View(curso);
        }

        // ======================
        // DELETE POST (CORREGIDO)
        // ======================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            // 🔥 CORRECCIÓN IMPORTANTE
            if (curso == null)
            {
                return NotFound();
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}