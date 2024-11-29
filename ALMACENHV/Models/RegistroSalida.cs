using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class RegistroSalida
    {
        [Key]
        public int RegistroSalidaID { get; set; }

        [NotMapped]
        public int Id { get => RegistroSalidaID; set => RegistroSalidaID = value; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        [Required]
        [StringLength(100)]
        public string NumeroDocumento { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Observaciones { get; set; }

        [Required]
        public int UbicacionID { get; set; }

        [Required]
        public int UsuarioID { get; set; }

        [ForeignKey("UbicacionID")]
        public virtual Ubicacion? Ubicacion { get; set; }

        [ForeignKey("UsuarioID")]
        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<RegistroSalidaDetalle> Detalles { get; set; } = new List<RegistroSalidaDetalle>();
    }
}
