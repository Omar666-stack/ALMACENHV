using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class RegistroIngreso
    {
        [Key]
        public int RegistroIngresoID { get; set; }

        [NotMapped]
        public int Id { get => RegistroIngresoID; set => RegistroIngresoID = value; }

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

        [ForeignKey("UsuarioID")]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("UbicacionID")]
        public virtual Ubicacion? Ubicacion { get; set; }

        public virtual ICollection<RegistroIngresoDetalle> Detalles { get; set; } = new List<RegistroIngresoDetalle>();
        public virtual ICollection<RegistroIngresoFoto> Fotos { get; set; } = new List<RegistroIngresoFoto>();
    }
}
