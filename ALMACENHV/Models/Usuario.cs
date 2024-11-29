using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ALMACENHV.Models
{
    [Table("Usuarios")]
    [Index(nameof(Email), IsUnique = true)]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido debe tener entre 3 y 100 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int RolID { get; set; }

        [ForeignKey("RolID")]
        public virtual Rol? Rol { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaModificacion { get; set; }
    }
}
