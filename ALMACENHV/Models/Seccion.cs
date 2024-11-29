using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ALMACENHV.Models
{
    [Table("Secciones")]
    [Index(nameof(Nombre), IsUnique = true)]
    public class Seccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeccionID { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
        [Column(TypeName = "varchar(50)")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 200 caracteres")]
        [Column(TypeName = "varchar(200)")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public bool Activo { get; set; } = true;

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<Ubicacion> Ubicaciones { get; set; } = new List<Ubicacion>();
    }
}
