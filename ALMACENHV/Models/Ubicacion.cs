using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ALMACENHV.Models
{
    [Table("Ubicaciones")]
    [Index(nameof(Codigo), IsUnique = true)]
    public class Ubicacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UbicacionID { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El código debe tener entre 3 y 20 caracteres")]
        [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "El código solo puede contener letras mayúsculas, números y guiones")]
        [Column(TypeName = "varchar(20)")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 200 caracteres")]
        [Column(TypeName = "varchar(200)")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La sección es requerida")]
        public int SeccionID { get; set; }

        [ForeignKey("SeccionID")]
        public virtual Seccion? Seccion { get; set; }

        [Required]
        public bool Activo { get; set; } = true;

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2")]
        public DateTime? FechaModificacion { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La capacidad debe ser un número positivo")]
        public int? Capacidad { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de ocupación debe estar entre 0 y 100")]
        public int? PorcentajeOcupacion { get; set; }

        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

        // Método para validar si la ubicación puede aceptar más productos
        public bool PuedeAceptarProductos(int cantidadNueva = 1)
        {
            if (!Capacidad.HasValue) return true;
            
            var ocupacionActual = Productos.Sum(p => p.Stock);
            return (ocupacionActual + cantidadNueva) <= Capacidad.Value;
        }

        // Método para actualizar el porcentaje de ocupación
        public void ActualizarPorcentajeOcupacion()
        {
            if (Capacidad.HasValue && Capacidad.Value > 0)
            {
                var ocupacionActual = Productos.Sum(p => p.Stock);
                PorcentajeOcupacion = (int)((ocupacionActual * 100.0) / Capacidad.Value);
            }
            else
            {
                PorcentajeOcupacion = null;
            }
        }
    }
}
