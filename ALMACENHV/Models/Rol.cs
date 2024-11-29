using System.ComponentModel.DataAnnotations;

namespace ALMACENHV.Models
{
    public class Rol
    {
        [Key]
        public int RolID { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descripcion { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
