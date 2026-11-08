using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_FernandezCastilloJosePablo.Data;

var builder = WebApplication.CreateBuilder(args);

// Cadena de conexión definida en appsettings.json
var cadenaConexion = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(cadenaConexion));

// ASP.NET Core Identity con roles (Administrador y Estudiante) y mensajes en español
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
    {
        opciones.Password.RequiredLength = 6;
        opciones.User.RequireUniqueEmail = true;
        opciones.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddErrorDescriber<DescriptorErroresEspanol>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opciones =>
{
    opciones.LoginPath = "/Cuenta/IniciarSesion";
    opciones.AccessDeniedPath = "/Cuenta/AccesoDenegado";
});

builder.Services.AddControllersWithViews(opciones =>
{
    opciones.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Crea la base de datos, roles, administrador y datos de ejemplo
using (var alcance = app.Services.CreateScope())
{
    await DbInicializador.InicializarAsync(alcance.ServiceProvider);
}

app.Run();
