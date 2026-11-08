using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_FernandezCastilloJosePablo.Models;

public class RegistroViewModel
{
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(120)]
    [Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [RegularExpression(@"^\d{9,12}$", ErrorMessage = "La cédula debe tener entre 9 y 12 dígitos, sin guiones.")]
    [Display(Name = "Cédula")]
    public string Cedula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione su carrera.")]
    [Display(Name = "Carrera")]
    public int CarreraId { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Contrasena { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirme la contraseña.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Contrasena), ErrorMessage = "Las contraseñas no coinciden.")]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmarContrasena { get; set; } = string.Empty;
}

public class LoginViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Contrasena { get; set; } = string.Empty;

    [Display(Name = "Recordarme")]
    public bool Recordarme { get; set; }
}

public class PanelViewModel
{
    public int TotalEstudiantes { get; set; }
    public int TotalCarreras { get; set; }
    public int TotalCursos { get; set; }
    public int TotalDocentes { get; set; }
    public int CursosSinDocente { get; set; }
    public List<Estudiante> UltimosEstudiantes { get; set; } = new();
}

public class CatalogoViewModel
{
    public Estudiante Estudiante { get; set; } = new();
    public List<Curso> Cursos { get; set; } = new();
}
