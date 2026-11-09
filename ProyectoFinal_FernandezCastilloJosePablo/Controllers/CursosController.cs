using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_FernandezCastilloJosePablo.Data;
using ProyectoFinal_FernandezCastilloJosePablo.Models;

namespace ProyectoFinal_FernandezCastilloJosePablo.Controllers;

[Authorize(Roles = "Administrador")]
public class CursosController : Controller
{
    private readonly ApplicationDbContext _db;

    public CursosController(ApplicationDbContext db) => _db = db;

    private async Task CargarListasAsync(int? carreraId = null, int? docenteId = null)
    {
        ViewBag.Carreras = new SelectList(await _db.Carreras.OrderBy(c => c.Nombre).ToListAsync(), "Id", "Nombre", carreraId);
        ViewBag.Docentes = new SelectList(await _db.Docentes.OrderBy(d => d.Nombre).ToListAsync(), "Id", "Nombre", docenteId);
    }

    public async Task<IActionResult> Index(string? buscar, int? carreraId)
    {
        var consulta = _db.Cursos.Include(c => c.Carrera).Include(c => c.Docente).AsQueryable();
        if (!string.IsNullOrWhiteSpace(buscar))
            consulta = consulta.Where(c => c.Codigo.Contains(buscar) || c.Nombre.Contains(buscar));
        if (carreraId.HasValue)
            consulta = consulta.Where(c => c.CarreraId == carreraId);

        await CargarListasAsync(carreraId);
        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(c => c.Codigo).ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        await CargarListasAsync();
        return View(new Curso());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Curso curso)
    {
        if (await _db.Cursos.AnyAsync(c => c.Codigo == curso.Codigo))
            ModelState.AddModelError(nameof(curso.Codigo), "Ya existe un curso con ese código.");
        if (!ModelState.IsValid)
        {
            await CargarListasAsync(curso.CarreraId, curso.DocenteId);
            return View(curso);
        }

        _db.Cursos.Add(curso);
        await _db.SaveChangesAsync();
        TempData["Exito"] = "El curso se registró correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var curso = await _db.Cursos.FindAsync(id);
        if (curso == null) return NotFound();
        await CargarListasAsync(curso.CarreraId, curso.DocenteId);
        return View(curso);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Curso curso)
    {
        if (id != curso.Id) return NotFound();
        if (await _db.Cursos.AnyAsync(c => c.Codigo == curso.Codigo && c.Id != id))
            ModelState.AddModelError(nameof(curso.Codigo), "Ya existe un curso con ese código.");
        if (!ModelState.IsValid)
        {
            await CargarListasAsync(curso.CarreraId, curso.DocenteId);
            return View(curso);
        }

        _db.Update(curso);
        await _db.SaveChangesAsync();
        TempData["Exito"] = "El curso se actualizó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var curso = await _db.Cursos.FindAsync(id);
        if (curso == null) return NotFound();

        if (await _db.Matriculas.AnyAsync(m => m.CursoId == id))
        {
            TempData["Error"] = "No se puede eliminar: el curso tiene estudiantes matriculados.";
            return RedirectToAction(nameof(Index));
        }

        _db.Cursos.Remove(curso);
        await _db.SaveChangesAsync();
        TempData["Exito"] = "El curso se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
