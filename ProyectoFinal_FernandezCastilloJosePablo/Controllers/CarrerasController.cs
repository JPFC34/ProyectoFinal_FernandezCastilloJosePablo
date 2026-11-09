using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_FernandezCastilloJosePablo.Data;
using ProyectoFinal_FernandezCastilloJosePablo.Models;

namespace ProyectoFinal_FernandezCastilloJosePablo.Controllers;

[Authorize(Roles = "Administrador")]
public class CarrerasController : Controller
{
    private readonly ApplicationDbContext _db;

    public CarrerasController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _db.Carreras.AsQueryable();
        if (!string.IsNullOrWhiteSpace(buscar))
            consulta = consulta.Where(c => c.Codigo.Contains(buscar) || c.Nombre.Contains(buscar));
        ViewBag.Buscar = buscar;
        return View(await consulta.OrderBy(c => c.Codigo).ToListAsync());
    }

    [HttpGet]
    public IActionResult Crear() => View(new Carrera());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Carrera carrera)
    {
        if (await _db.Carreras.AnyAsync(c => c.Codigo == carrera.Codigo))
            ModelState.AddModelError(nameof(carrera.Codigo), "Ya existe una carrera con ese código.");
        if (!ModelState.IsValid) return View(carrera);

        _db.Carreras.Add(carrera);
        await _db.SaveChangesAsync();
        TempData["Exito"] = "La carrera se registró correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var carrera = await _db.Carreras.FindAsync(id);
        if (carrera == null) return NotFound();
        return View(carrera);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Carrera carrera)
    {
        if (id != carrera.Id) return NotFound();
        if (await _db.Carreras.AnyAsync(c => c.Codigo == carrera.Codigo && c.Id != id))
            ModelState.AddModelError(nameof(carrera.Codigo), "Ya existe una carrera con ese código.");
        if (!ModelState.IsValid) return View(carrera);

        _db.Update(carrera);
        await _db.SaveChangesAsync();
        TempData["Exito"] = "La carrera se actualizó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var carrera = await _db.Carreras.FindAsync(id);
        if (carrera == null) return NotFound();

        if (await _db.Cursos.AnyAsync(c => c.CarreraId == id) || await _db.Estudiantes.AnyAsync(e => e.CarreraId == id))
        {
            TempData["Error"] = "No se puede eliminar: la carrera tiene cursos o estudiantes asociados.";
            return RedirectToAction(nameof(Index));
        }

        _db.Carreras.Remove(carrera);
        await _db.SaveChangesAsync();
        TempData["Exito"] = "La carrera se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
