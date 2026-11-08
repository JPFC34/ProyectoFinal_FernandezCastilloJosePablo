using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_FernandezCastilloJosePablo.Models;

public class Curso
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(10, ErrorMessage = "El código admite máximo 10 caracteres.")]
    [Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre del curso es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre admite máximo 100 caracteres.")]
    [Display(Name = "Nombre del curso")]
    public string Nombre { get; set; } = string.Empty;

    [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10.")]
    [Display(Name = "Créditos")]
    public int Creditos { get; set; } = 3;

    [Range(1, 200, ErrorMessage = "El cupo debe estar entre 1 y 200.")]
    [Display(Name = "Cupo máximo")]
    public int CupoMaximo { get; set; } = 30;

    [Required(ErrorMessage = "El horario es obligatorio.")]
    [StringLength(60, ErrorMessage = "El horario admite máximo 60 caracteres.")]
    [Display(Name = "Horario")]
    public string Horario { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una carrera.")]
    [Display(Name = "Carrera")]
    public int CarreraId { get; set; }
    public Carrera? Carrera { get; set; }

    [Display(Name = "Docente")]
    public int? DocenteId { get; set; }
    public Docente? Docente { get; set; }

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
