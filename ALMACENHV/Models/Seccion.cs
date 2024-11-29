using System.ComponentModel.DataAnnotations;

namespace ALMACENHV.Models
{
    public class Seccion
    {
        [Key]
        public int SeccionID { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        public virtual ICollection<Ubicacion> Ubicaciones { get; set; } = new List<Ubicacion>();
    }
}
