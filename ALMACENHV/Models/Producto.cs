using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ALMACENHV.Models
{
    [Table("Productos")]
    [Index(nameof(Codigo), IsUnique = true)]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductoID { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El código debe tener entre 3 y 20 caracteres")]
        [Column(TypeName = "varchar(20)")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 200 caracteres")]
        [Column(TypeName = "varchar(200)")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int Stock { get; set; }

        [Required]
        public int StockMinimo { get; set; }

        [Required]
        public int UbicacionID { get; set; }

        [ForeignKey("UbicacionID")]
        public virtual Ubicacion? Ubicacion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaModificacion { get; set; }
    }
}
