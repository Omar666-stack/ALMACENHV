using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class RegistroIngresoDetalle
    {
        [Key]
        public int RegistroIngresoDetalleID { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int RegistroIngresoID { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto? Producto { get; set; }

        [ForeignKey("RegistroIngresoID")]
        public virtual RegistroIngreso? RegistroIngreso { get; set; }
    }
}
