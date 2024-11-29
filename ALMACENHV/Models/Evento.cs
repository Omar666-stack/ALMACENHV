using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    [Table("Eventos")]
    public class Evento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventoID { get; set; }

        [Required]
        public int UsuarioID { get; set; }

        [ForeignKey("UsuarioID")]
        public virtual Usuario? Usuario { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [StringLength(50, ErrorMessage = "El tipo de evento no puede exceder los 50 caracteres")]
        public string TipoEvento { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(200)")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Activo { get; set; } = true;
    }
}
