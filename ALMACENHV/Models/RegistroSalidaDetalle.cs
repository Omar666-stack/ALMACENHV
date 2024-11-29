using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class RegistroSalidaDetalle
    {
        [Key]
        public int RegistroSalidaDetalleID { get; set; }

        [NotMapped]
        public int Id { get => RegistroSalidaDetalleID; set => RegistroSalidaDetalleID = value; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int RegistroSalidaID { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto? Producto { get; set; }

        [ForeignKey("RegistroSalidaID")]
        public virtual RegistroSalida? RegistroSalida { get; set; }
    }
}
