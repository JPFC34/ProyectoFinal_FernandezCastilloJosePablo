using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_FernandezCastilloJosePablo.Models;

public class Carrera
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(10, ErrorMessage = "El código admite máximo 10 caracteres.")]
    [Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre de la carrera es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre admite máximo 100 caracteres.")]
    [Display(Name = "Nombre de la carrera")]
    public string Nombre { get; set; } = string.Empty;

    [Range(1, 20, ErrorMessage = "Ingrese una cantidad de ciclos entre 1 y 20.")]
    [Display(Name = "Cantidad de ciclos")]
    public int CantidadCiclos { get; set; } = 8;

    [Display(Name = "Activa")]
    public bool Activa { get; set; } = true;

    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
    public ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
}
