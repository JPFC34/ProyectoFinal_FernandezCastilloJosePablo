using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_FernandezCastilloJosePablo.Models;

namespace ProyectoFinal_FernandezCastilloJosePablo.Data;

// Crea la base de datos, los roles, el administrador inicial y datos de ejemplo
public static class DbInicializador
{
    public const string RolAdmin = "Administrador";
    public const string RolEstudiante = "Estudiante";
    public const string CorreoAdmin = "admin@uhcr.ac.cr";
    public const string ClaveAdmin = "Admin123!";

    public static async Task InicializarAsync(IServiceProvider servicios)
    {
        var db = servicios.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        var roles = servicios.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var rol in new[] { RolAdmin, RolEstudiante })
        {
            if (!await roles.RoleExistsAsync(rol))
                await roles.CreateAsync(new IdentityRole(rol));
        }

        var usuarios = servicios.GetRequiredService<UserManager<IdentityUser>>();
        if (await usuarios.FindByEmailAsync(CorreoAdmin) == null)
        {
            var admin = new IdentityUser { UserName = CorreoAdmin, Email = CorreoAdmin, EmailConfirmed = true };
            var resultado = await usuarios.CreateAsync(admin, ClaveAdmin);
            if (resultado.Succeeded)
                await usuarios.AddToRoleAsync(admin, RolAdmin);
        }

        if (!await db.Carreras.AnyAsync())
        {
            var sis = new Carrera { Codigo = "SIS", Nombre = "Ingeniería en Sistemas", CantidadCiclos = 8 };
            var adm = new Carrera { Codigo = "ADM", Nombre = "Administración de Empresas", CantidadCiclos = 8 };
            var der = new Carrera { Codigo = "DER", Nombre = "Derecho", CantidadCiclos = 10 };
            db.Carreras.AddRange(sis, adm, der);

            var rojas = new Docente { Nombre = "María Rojas", Correo = "mrojas@uhcr.ac.cr", Especialidad = "Programación" };
            var vega = new Docente { Nombre = "Luis Vega", Correo = "lvega@uhcr.ac.cr", Especialidad = "Bases de datos" };
            var mora = new Docente { Nombre = "Ana Mora", Correo = "amora@uhcr.ac.cr", Especialidad = "Matemática" };
            var solis = new Docente { Nombre = "Jorge Solís", Correo = "jsolis@uhcr.ac.cr", Especialidad = "Redes" };
            db.Docentes.AddRange(rojas, vega, mora, solis);

            db.Cursos.AddRange(
                new Curso { Codigo = "SIS-101", Nombre = "Programación Orientada a Objetos", Creditos = 4, CupoMaximo = 30, Horario = "L-J 8:00", Carrera = sis, Docente = rojas },
                new Curso { Codigo = "MAT-110", Nombre = "Cálculo I", Creditos = 4, CupoMaximo = 30, Horario = "M-V 9:00", Carrera = sis, Docente = mora },
                new Curso { Codigo = "SIS-120", Nombre = "Bases de Datos", Creditos = 3, CupoMaximo = 25, Horario = "K 14:00", Carrera = sis, Docente = vega },
                new Curso { Codigo = "SIS-130", Nombre = "Redes de Computadoras", Creditos = 3, CupoMaximo = 25, Horario = "V 18:00", Carrera = sis, Docente = solis },
                new Curso { Codigo = "ADM-101", Nombre = "Contabilidad I", Creditos = 3, CupoMaximo = 30, Horario = "L-M 10:00", Carrera = adm },
                new Curso { Codigo = "ADM-110", Nombre = "Administración General", Creditos = 3, CupoMaximo = 30, Horario = "K-J 10:00", Carrera = adm },
                new Curso { Codigo = "DER-101", Nombre = "Introducción al Derecho", Creditos = 3, CupoMaximo = 35, Horario = "L-M 16:00", Carrera = der });

            await db.SaveChangesAsync();
        }
    }
}
