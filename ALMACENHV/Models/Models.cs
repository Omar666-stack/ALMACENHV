namespace ALMACENHV.Models
{
    public class Cargo
    {
        public int CargoID { get; set; }
        public string NombreCargo { get; set; }
        public string Descripcion { get; set; }
    }

    public class Producto
    {
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int SeccionID { get; set; }
        public Seccion Seccion { get; set; }
    }

    public class Apunte
    {
        public int ApunteID { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RolID { get; set; }
        public Rol Rol { get; set; }
        public int CargoID { get; set; }
        public Cargo Cargo { get; set; }
    }

    public class Rol
    {
        public int RolID { get; set; }
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }
    }

    public class Evento
    {
        public int EventoID { get; set; }
        public string NombreEvento { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class RegistroIngresoDetalle
    {
        public int RegistroIngresoDetalleID { get; set; }
        public int RegistroIngresoID { get; set; }
        public RegistroEntrada RegistroIngreso { get; set; }
        public int ProductoID { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
    }

    public class RegistroSalidaDetalle
    {
        public int RegistroSalidaDetalleID { get; set; }
        public int RegistroSalidaID { get; set; }
        public RegistroSalida RegistroSalida { get; set; }
        public int ProductoID { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
    }

    public class RegistroEntrada
    {
        public int RegistroEntradaID { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        public int ProveedorID { get; set; }
        public Proveedor Proveedor { get; set; }
    }

    public class RegistroSalida
    {
        public int RegistroSalidaID { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        public int UbicacionID { get; set; }
        public Ubicacion Ubicacion { get; set; }
    }

    public class Proveedor
    {
        public int ProveedorID { get; set; }
        public string NombreProveedor { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }

    public class Seccion
    {
        public int SeccionID { get; set; }
        public string NombreSeccion { get; set; }
        public string Descripcion { get; set; }
    }

    public class Ubicacion
    {
        public int UbicacionID { get; set; }
        public string NombreUbicacion { get; set; }
        public string Descripcion { get; set; }
    }

    public class RegistroIngresoFoto
    {
        public int RegistroIngresoFotoID { get; set; }
        public int RegistroIngresoID { get; set; }
        public RegistroEntrada RegistroIngreso { get; set; }
        public string FotoURL { get; set; }
        public string Descripcion { get; set; }
    }
}