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

        // LISTA + FILTRO
        public async Task<IActionResult> Index(string nombre, int? creditos)
        {
            var cursos = _context.Cursos.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                cursos = cursos.Where(c => c.Nombre.Contains(nombre));
            }

            if (creditos.HasValue)
            {
                cursos = cursos.Where(c => c.Creditos == creditos);
            }

            return View(await cursos.Where(c => c.Activo).ToListAsync());
        }

        // DETALLE
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null) return NotFound();

            return View(curso);
        }

        // CREAR (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREAR (POST)
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
    }
}