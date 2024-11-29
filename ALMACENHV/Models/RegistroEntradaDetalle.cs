using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class RegistroEntradaDetalle
    {
        [Key]
        public int RegistroEntradaDetalleID { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int RegistroEntradaID { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto? Producto { get; set; }

        [ForeignKey("RegistroEntradaID")]
        public virtual RegistroEntrada? RegistroEntrada { get; set; }
    }
}
