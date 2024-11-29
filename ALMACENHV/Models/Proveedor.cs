using System.ComponentModel.DataAnnotations;

namespace ALMACENHV.Models
{
    public class Proveedor
    {
        [Key]
        public int ProveedorID { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreEmpresa { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string RUC { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string? PersonaContacto { get; set; }

        [StringLength(100)]
        public string? ContactoNombre { get; set; }

        public bool Estado { get; set; } = true;

        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

        public virtual ICollection<RegistroEntrada> RegistroIngresos { get; set; } = new List<RegistroEntrada>();
    }
}
