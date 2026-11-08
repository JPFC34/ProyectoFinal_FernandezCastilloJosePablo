using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_FernandezCastilloJosePablo.Data;
using ProyectoFinal_FernandezCastilloJosePablo.Models;

namespace ProyectoFinal_FernandezCastilloJosePablo.Controllers;

[Authorize(Roles = "Administrador")]
public class PanelController : Controller
{
    private readonly ApplicationDbContext _db;

    public PanelController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var modelo = new PanelViewModel
        {
            TotalEstudiantes = await _db.Estudiantes.CountAsync(),
            TotalCarreras = await _db.Carreras.CountAsync(),
            TotalCursos = await _db.Cursos.CountAsync(),
            TotalDocentes = await _db.Docentes.CountAsync(),
            CursosSinDocente = await _db.Cursos.CountAsync(c => c.DocenteId == null),
            UltimosEstudiantes = await _db.Estudiantes.Include(e => e.Carrera)
                .OrderByDescending(e => e.Id).Take(5).ToListAsync()
        };
        return View(modelo);
    }
}
