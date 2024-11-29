using ALMACENHV.Models;
using BCrypt.Net;

namespace ALMACENHV.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AlmacenContext context)
        {
            context.Database.EnsureCreated();

            // Verificar si ya hay datos
            if (context.Roles.Any())
            {
                return;   // La base de datos ya tiene datos
            }

            // Roles
            var roles = new Rol[]
            {
                new Rol { RolID = 1, Nombre = "Administrador", Descripcion = "Acceso total al sistema" },
                new Rol { RolID = 2, Nombre = "Supervisor", Descripcion = "Supervisión de operaciones" },
                new Rol { RolID = 3, Nombre = "Operador", Descripcion = "Operaciones básicas" }
            };

            foreach (Rol r in roles)
            {
                context.Roles.Add(r);
            }
            context.SaveChanges();

            // Usuarios
            var usuarios = new Usuario[]
            {
                new Usuario {
                    UsuarioID = 1,
                    Nombre = "Admin",
                    Email = "admin@almacenhv.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    RolID = 1,
                    Activo = true
                },
                new Usuario {
                    UsuarioID = 2,
                    Nombre = "Supervisor",
                    Email = "supervisor@almacenhv.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Super123!"),
                    RolID = 2,
                    Activo = true
                },
                new Usuario {
                    UsuarioID = 3,
                    Nombre = "Operador",
                    Email = "operador@almacenhv.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Oper123!"),
                    RolID = 3,
                    Activo = true
                }
            };

            foreach (Usuario u in usuarios)
            {
                context.Usuarios.Add(u);
            }
            context.SaveChanges();

            // Secciones
            var secciones = new Seccion[]
            {
                new Seccion { SeccionID = 1, Nombre = "Almacén Principal", Descripcion = "Sección principal del almacén", Activo = true },
                new Seccion { SeccionID = 2, Nombre = "Almacén Secundario", Descripcion = "Sección secundaria del almacén", Activo = true },
                new Seccion { SeccionID = 3, Nombre = "Zona de Picking", Descripcion = "Área de preparación de pedidos", Activo = true }
            };

            foreach (Seccion s in secciones)
            {
                context.Secciones.Add(s);
            }
            context.SaveChanges();

            // Ubicaciones
            var ubicaciones = new Ubicacion[]
            {
                new Ubicacion { 
                    UbicacionID = 1, 
                    Codigo = "A-01", 
                    Descripcion = "Estantería A, Nivel 1", 
                    SeccionID = 1, 
                    Activo = true,
                    Capacidad = 1000,
                    PorcentajeOcupacion = 0
                },
                new Ubicacion { 
                    UbicacionID = 2, 
                    Codigo = "A-02", 
                    Descripcion = "Estantería A, Nivel 2", 
                    SeccionID = 1, 
                    Activo = true,
                    Capacidad = 1000,
                    PorcentajeOcupacion = 0
                },
                new Ubicacion { 
                    UbicacionID = 3, 
                    Codigo = "B-01", 
                    Descripcion = "Estantería B, Nivel 1", 
                    SeccionID = 2, 
                    Activo = true,
                    Capacidad = 800,
                    PorcentajeOcupacion = 0
                },
                new Ubicacion { 
                    UbicacionID = 4, 
                    Codigo = "B-02", 
                    Descripcion = "Estantería B, Nivel 2", 
                    SeccionID = 2, 
                    Activo = true,
                    Capacidad = 800,
                    PorcentajeOcupacion = 0
                },
                new Ubicacion { 
                    UbicacionID = 5, 
                    Codigo = "P-01", 
                    Descripcion = "Zona Picking 1", 
                    SeccionID = 3, 
                    Activo = true,
                    Capacidad = 500,
                    PorcentajeOcupacion = 0
                }
            };

            foreach (Ubicacion u in ubicaciones)
            {
                context.Ubicaciones.Add(u);
            }
            context.SaveChanges();

            // Productos
            var productos = new Producto[]
            {
                new Producto {
                    ProductoID = 1,
                    Codigo = "PROD001",
                    Nombre = "Producto 1",
                    Descripcion = "Descripción del Producto 1",
                    Stock = 100,
                    StockMinimo = 10,
                    UbicacionID = 1,
                    Activo = true
                },
                new Producto {
                    ProductoID = 2,
                    Codigo = "PROD002",
                    Nombre = "Producto 2",
                    Descripcion = "Descripción del Producto 2",
                    Stock = 150,
                    StockMinimo = 15,
                    UbicacionID = 2,
                    Activo = true
                },
                new Producto {
                    ProductoID = 3,
                    Codigo = "PROD003",
                    Nombre = "Producto 3",
                    Descripcion = "Descripción del Producto 3",
                    Stock = 75,
                    StockMinimo = 8,
                    UbicacionID = 3,
                    Activo = true
                }
            };

            foreach (Producto p in productos)
            {
                context.Productos.Add(p);
            }
            context.SaveChanges();
        }
    }
}
