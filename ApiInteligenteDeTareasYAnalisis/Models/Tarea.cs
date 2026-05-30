using System.ComponentModel.DataAnnotations;

namespace ApiInteligenteDeTareasYAnalisis.Models
{
    public class Tarea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 500 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [RegularExpression("^(Pendiente|EnProceso|Completada)$", 
            ErrorMessage = "El estado debe ser: Pendiente, EnProceso o Completada")]
        public string Estado { get; set; } = "Pendiente";

        [Required(ErrorMessage = "La prioridad es obligatoria")]
        [RegularExpression("^(Baja|Media|Alta)$", 
            ErrorMessage = "La prioridad debe ser: Baja, Media o Alta")]
        public string Prioridad { get; set; } = "Media";

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime FechaVencimiento { get; set; }
    }
}
