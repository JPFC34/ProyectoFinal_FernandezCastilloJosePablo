using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_FernandezCastilloJosePablo.Data;
using ProyectoFinal_FernandezCastilloJosePablo.Models;

namespace ProyectoFinal_FernandezCastilloJosePablo.Controllers;

[Authorize(Roles = "Administrador")]
public class DocentesController : Controller
{
    private readonly ApplicationDbContext _db;

    public DocentesController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _db.Docentes.Include(d => d.Cursos).AsQueryable();
        if (!string.IsNullOrWhiteSpace(buscar))
            consulta = consulta.Where(d => d.Nombre.Contains(buscar) || d.Especialidad.Contains(buscar) || d.Correo.Contains(buscar));
        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(d => d.Nombre).ToListAsync());
    }

    [HttpGet]
    public IActionResult Crear() => View(new Docente());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Docente docente)
    {
        if (await _db.Docentes.AnyAsync(d => d.Correo == docente.Correo))
            ModelState.AddModelError(nameof(docente.Correo), "Ya existe un docente con ese correo.");
        if (!ModelState.IsValid) return View(docente);

        _db.Docentes.Add(docente);
        await _db.SaveChangesAsync();
        TempData["Exito"] = "El docente se registró correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var docente = await _db.Docentes.FindAsync(id);
        if (docente == null) return NotFound();
        return View(docente);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Docente docente)
    {
        if (id != docente.Id) return NotFound();
        if (await _db.Docentes.AnyAsync(d => d.Correo == docente.Correo && d.Id != id))
            ModelState.AddModelError(nameof(docente.Correo), "Ya existe un docente con ese correo.");
        if (!ModelState.IsValid) return View(docente);

        _db.Update(docente);
        await _db.SaveChangesAsync();
        TempData["Exito"] = "El docente se actualizó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var docente = await _db.Docentes.FindAsync(id);
        if (docente == null) return NotFound();

        if (await _db.Cursos.AnyAsync(c => c.DocenteId == id))
        {
            TempData["Error"] = "No se puede eliminar: el docente tiene cursos asignados.";
            return RedirectToAction(nameof(Index));
        }

        _db.Docentes.Remove(docente);
        await _db.SaveChangesAsync();
        TempData["Exito"] = "El docente se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
