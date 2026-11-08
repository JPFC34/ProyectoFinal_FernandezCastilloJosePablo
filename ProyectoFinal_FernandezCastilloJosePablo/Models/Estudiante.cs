using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_FernandezCastilloJosePablo.Models;

public class Estudiante
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    [Display(Name = "Cédula")]
    public string Cedula { get; set; } = string.Empty;

    [Required, StringLength(120)]
    [Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(100)]
    [Display(Name = "Correo")]
    public string Correo { get; set; } = string.Empty;

    [Display(Name = "Carrera")]
    public int CarreraId { get; set; }
    public Carrera? Carrera { get; set; }

    // Id del usuario de ASP.NET Core Identity asociado a este estudiante
    [StringLength(450)]
    public string UsuarioId { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
