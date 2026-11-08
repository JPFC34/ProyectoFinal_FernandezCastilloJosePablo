using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_FernandezCastilloJosePablo.Models;

public class Docente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre admite máximo 100 caracteres.")]
    [Display(Name = "Nombre completo")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
    [StringLength(100)]
    [Display(Name = "Correo institucional")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    [StringLength(100, ErrorMessage = "La especialidad admite máximo 100 caracteres.")]
    [Display(Name = "Especialidad")]
    public string Especialidad { get; set; } = string.Empty;

    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}
