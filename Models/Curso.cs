using System.ComponentModel.DataAnnotations;

namespace PortalAcademico.Models;

public class Curso
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Range(1, 10)]
    public int Creditos { get; set; }

    // Relación
    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}