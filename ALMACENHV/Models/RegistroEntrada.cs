using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    [Table("RegistroEntradas")]
    public class RegistroEntrada
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegistroEntradaID { get; set; }

        [Required]
        public int UsuarioID { get; set; }

        [ForeignKey("UsuarioID")]
        public virtual Usuario? Usuario { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        [StringLength(200, ErrorMessage = "La observación no puede exceder los 200 caracteres")]
        public string Observacion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Activo { get; set; } = true;

        public virtual ICollection<RegistroEntradaDetalle> Detalles { get; set; } = new List<RegistroEntradaDetalle>();
    }
}
