using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_FernandezCastilloJosePablo.Data;
using ProyectoFinal_FernandezCastilloJosePablo.Models;

namespace ProyectoFinal_FernandezCastilloJosePablo.Controllers;

[Authorize(Roles = "Estudiante")]
public class CatalogoController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _usuarios;

    public CatalogoController(ApplicationDbContext db, UserManager<IdentityUser> usuarios)
    {
        _db = db;
        _usuarios = usuarios;
    }

    public async Task<IActionResult> Index()
    {
        var usuarioId = _usuarios.GetUserId(User);
        var estudiante = await _db.Estudiantes.Include(e => e.Carrera)
            .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);
        if (estudiante == null) return RedirectToAction("AccesoDenegado", "Cuenta");

        var cursos = await _db.Cursos.Include(c => c.Docente).Include(c => c.Matriculas)
            .Where(c => c.CarreraId == estudiante.CarreraId)
            .OrderBy(c => c.Codigo).ToListAsync();

        return View(new CatalogoViewModel { Estudiante = estudiante, Cursos = cursos });
    }
}
