using System.ComponentModel.DataAnnotations;

namespace ALMACENHV.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido")]
        public int RolID { get; set; }

        [Required(ErrorMessage = "El cargo es requerido")]
        public int CargoID { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "El ID de usuario es requerido")]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "La contraseña actual es requerida")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres", MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "El ID de usuario es requerido")]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres", MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
