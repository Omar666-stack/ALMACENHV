using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class Ubicacion
    {
        [Key]
        public int UbicacionID { get; set; }

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public int Capacidad { get; set; }

        [Required]
        public int SeccionID { get; set; }

        [ForeignKey("SeccionID")]
        public virtual Seccion? Seccion { get; set; }

        public bool Estado { get; set; } = true;

        public virtual ICollection<RegistroEntrada> RegistroEntradas { get; set; } = new List<RegistroEntrada>();
        public virtual ICollection<RegistroSalida> RegistroSalidas { get; set; } = new List<RegistroSalida>();
    }
}
