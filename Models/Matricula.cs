using System.ComponentModel.DataAnnotations;

namespace PortalAcademico.Models
{
    public class Matricula
    {
        public int Id { get; set; }

        // FK
        [Required]
        public int CursoId { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;

        // Fecha automática
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Estado controlado
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "Activo";

        // Navegación
        public Curso Curso { get; set; } = null!;
    }
}