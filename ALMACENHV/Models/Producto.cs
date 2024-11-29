using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class Producto
    {
        [Key]
        public int ProductoID { get; set; }

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(50)]
        public string CodigoInterno { get; set; } = string.Empty;

        [Required]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int StockMinimo { get; set; }

        [Required]
        public int ProveedorID { get; set; }

        [Required]
        public int UbicacionID { get; set; }

        [Required]
        public int SeccionID { get; set; }

        [ForeignKey("ProveedorID")]
        public virtual Proveedor? Proveedor { get; set; }

        [ForeignKey("UbicacionID")]
        public virtual Ubicacion? Ubicacion { get; set; }

        [ForeignKey("SeccionID")]
        public virtual Seccion? Seccion { get; set; }

        public virtual ICollection<RegistroEntradaDetalle> RegistroEntradaDetalles { get; set; } = new List<RegistroEntradaDetalle>();
        public virtual ICollection<RegistroSalidaDetalle> RegistroSalidaDetalles { get; set; } = new List<RegistroSalidaDetalle>();
        public virtual ICollection<RegistroIngresoDetalle> RegistroIngresoDetalles { get; set; } = new List<RegistroIngresoDetalle>();
    }
}
