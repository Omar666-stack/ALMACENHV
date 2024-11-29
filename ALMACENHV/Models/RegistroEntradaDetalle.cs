using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    [Table("RegistroEntradaDetalles")]
    public class RegistroEntradaDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegistroEntradaDetalleID { get; set; }

        [Required]
        public int RegistroEntradaID { get; set; }

        [ForeignKey("RegistroEntradaID")]
        public virtual RegistroEntrada? RegistroEntrada { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto? Producto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        [StringLength(200, ErrorMessage = "La observación no puede exceder los 200 caracteres")]
        public string Observacion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Activo { get; set; } = true;
    }
}
