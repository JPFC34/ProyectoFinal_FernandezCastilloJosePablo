using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_FernandezCastilloJosePablo.Models;

public class Matricula
{
    public int Id { get; set; }

    public int EstudianteId { get; set; }
    public Estudiante? Estudiante { get; set; }

    public int CursoId { get; set; }
    public Curso? Curso { get; set; }

    [Display(Name = "Fecha de matrícula")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [StringLength(30)]
    [Display(Name = "Período")]
    public string Periodo { get; set; } = "I Ciclo 2026";
}
