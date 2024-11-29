using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMACENHV.Models
{
    public class RegistroIngresoFoto
    {
        [Key]
        public int RegistroIngresoFotoID { get; set; }

        [NotMapped]
        public int Id { get => RegistroIngresoFotoID; set => RegistroIngresoFotoID = value; }

        [Required]
        public string RutaArchivo { get; set; } = string.Empty;

        [Required]
        public DateTime FechaCarga { get; set; }

        [Required]
        public int RegistroIngresoID { get; set; }

        [ForeignKey("RegistroIngresoID")]
        public virtual RegistroIngreso? RegistroIngreso { get; set; }
    }
}
