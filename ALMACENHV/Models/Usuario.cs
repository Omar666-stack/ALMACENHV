using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Apellido { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public int RolID { get; set; }
        public int CargoID { get; set; }

        [ForeignKey("RolID")]
        public virtual Rol? Rol { get; set; }

        [ForeignKey("CargoID")]
        public virtual Cargo? Cargo { get; set; }

        public virtual ICollection<Apunte> Apuntes { get; set; } = new List<Apunte>();
        public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
        public virtual ICollection<RegistroEntrada> RegistroEntradas { get; set; } = new List<RegistroEntrada>();
        public virtual ICollection<RegistroSalida> RegistroSalidas { get; set; } = new List<RegistroSalida>();
        public virtual ICollection<RegistroIngreso> RegistroIngresos { get; set; } = new List<RegistroIngreso>();
    }
}
