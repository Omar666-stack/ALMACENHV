using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class RegistroEntrada
    {
        [Key]
        public int RegistroEntradaID { get; set; }

        [NotMapped]
        public int Id { get => RegistroEntradaID; set => RegistroEntradaID = value; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        [Required]
        [StringLength(100)]
        public string NumeroDocumento { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Observaciones { get; set; }

        [Required]
        public int UsuarioID { get; set; }

        [Required]
        public int UbicacionID { get; set; }

        [Required]
        public int ProveedorID { get; set; }

        [ForeignKey("UsuarioID")]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("UbicacionID")]
        public virtual Ubicacion? Ubicacion { get; set; }

        [ForeignKey("ProveedorID")]
        public virtual Proveedor? Proveedor { get; set; }

        public virtual ICollection<RegistroEntradaDetalle> Detalles { get; set; } = new List<RegistroEntradaDetalle>();
    }
}
