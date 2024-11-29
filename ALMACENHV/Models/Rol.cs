using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ALMACENHV.Models
{
    [Table("Roles")]
    [Index(nameof(Nombre), IsUnique = true)]
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(200)")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 200 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public bool Activo { get; set; } = true;

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
