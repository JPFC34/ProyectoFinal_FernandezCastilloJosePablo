using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_FernandezCastilloJosePablo.Data;
using ProyectoFinal_FernandezCastilloJosePablo.Models;

namespace ProyectoFinal_FernandezCastilloJosePablo.Controllers;

public class CuentaController : Controller
{
    private readonly UserManager<IdentityUser> _usuarios;
    private readonly SignInManager<IdentityUser> _sesion;
    private readonly ApplicationDbContext _db;

    public CuentaController(UserManager<IdentityUser> usuarios, SignInManager<IdentityUser> sesion, ApplicationDbContext db)
    {
        _usuarios = usuarios;
        _sesion = sesion;
        _db = db;
    }

    private async Task CargarCarrerasAsync(int? seleccionada = null)
    {
        var carreras = await _db.Carreras.Where(c => c.Activa).OrderBy(c => c.Nombre).ToListAsync();
        ViewBag.Carreras = new SelectList(carreras, "Id", "Nombre", seleccionada);
    }

    [HttpGet]
    public async Task<IActionResult> Registro()
    {
        await CargarCarrerasAsync();
        return View(new RegistroViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(RegistroViewModel modelo)
    {
        if (await _db.Estudiantes.AnyAsync(e => e.Cedula == modelo.Cedula))
            ModelState.AddModelError(nameof(modelo.Cedula), "Ya existe un estudiante registrado con esa cédula.");

        if (modelo.CarreraId > 0 && !await _db.Carreras.AnyAsync(c => c.Id == modelo.CarreraId && c.Activa))
            ModelState.AddModelError(nameof(modelo.CarreraId), "La carrera seleccionada no está disponible.");

        if (!ModelState.IsValid)
        {
            await CargarCarrerasAsync(modelo.CarreraId);
            return View(modelo);
        }

        var usuario = new IdentityUser { UserName = modelo.Correo, Email = modelo.Correo };
        var resultado = await _usuarios.CreateAsync(usuario, modelo.Contrasena);
        if (!resultado.Succeeded)
        {
            foreach (var error in resultado.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            await CargarCarrerasAsync(modelo.CarreraId);
            return View(modelo);
        }

        await _usuarios.AddToRoleAsync(usuario, DbInicializador.RolEstudiante);

        _db.Estudiantes.Add(new Estudiante
        {
            Cedula = modelo.Cedula,
            NombreCompleto = modelo.NombreCompleto,
            Correo = modelo.Correo,
            CarreraId = modelo.CarreraId,
            UsuarioId = usuario.Id
        });
        await _db.SaveChangesAsync();

        await _sesion.SignInAsync(usuario, isPersistent: false);
        TempData["Exito"] = "¡Tu cuenta fue creada con éxito! Bienvenido a la UHCR.";
        return RedirectToAction("Index", "Catalogo");
    }

    [HttpGet]
    public IActionResult IniciarSesion(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> IniciarSesion(LoginViewModel modelo, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(modelo);

        var resultado = await _sesion.PasswordSignInAsync(modelo.Correo, modelo.Contrasena, modelo.Recordarme, lockoutOnFailure: false);
        if (!resultado.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(modelo);
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        var usuario = await _usuarios.FindByEmailAsync(modelo.Correo);
        if (usuario != null && await _usuarios.IsInRoleAsync(usuario, DbInicializador.RolAdmin))
            return RedirectToAction("Index", "Panel");

        return RedirectToAction("Index", "Catalogo");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CerrarSesion()
    {
        await _sesion.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccesoDenegado() => View();
}
