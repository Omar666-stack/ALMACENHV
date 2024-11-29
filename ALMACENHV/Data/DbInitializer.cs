using ALMACENHV.Models;
using Microsoft.EntityFrameworkCore;

namespace ALMACENHV.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AlmacenContext context)
        {
            // Ensure we have the database
            context.Database.EnsureCreated();

            // Check if we already have any roles
            if (!await context.Roles.AnyAsync())
            {
                var roles = new[]
                {
                    new Rol { Nombre = "Administrador", Descripcion = "Acceso total al sistema" },
                    new Rol { Nombre = "Usuario", Descripcion = "Acceso limitado al sistema" }
                };

                await context.Roles.AddRangeAsync(roles);
            }

            // Check if we already have any cargos
            if (!await context.Cargos.AnyAsync())
            {
                var cargos = new[]
                {
                    new Cargo { Nombre = "Gerente", Descripcion = "Gerente de almacén" },
                    new Cargo { Nombre = "Operador", Descripcion = "Operador de almacén" }
                };

                await context.Cargos.AddRangeAsync(cargos);
            }

            // Check if we already have any secciones
            if (!await context.Secciones.AnyAsync())
            {
                var secciones = new[]
                {
                    new Seccion { Nombre = "General", Descripcion = "Sección general del almacén" },
                    new Seccion { Nombre = "Refrigerados", Descripcion = "Sección de productos refrigerados" }
                };

                await context.Secciones.AddRangeAsync(secciones);
            }

            // Check if we already have any ubicaciones
            if (!await context.Ubicaciones.AnyAsync())
            {
                // Obtener la sección general
                var seccionGeneral = await context.Secciones.FirstOrDefaultAsync(s => s.Nombre == "General");
                if (seccionGeneral != null)
                {
                    var ubicaciones = new[]
                    {
                        new Ubicacion { 
                            Codigo = "AP001",
                            Nombre = "Almacén Principal",
                            Capacidad = 1000,
                            SeccionID = seccionGeneral.SeccionID
                        },
                        new Ubicacion { 
                            Codigo = "AS001",
                            Nombre = "Almacén Secundario",
                            Capacidad = 500,
                            SeccionID = seccionGeneral.SeccionID
                        }
                    };

                    await context.Ubicaciones.AddRangeAsync(ubicaciones);
                }
            }

            // Save all the changes
            await context.SaveChangesAsync();
        }
    }
}
