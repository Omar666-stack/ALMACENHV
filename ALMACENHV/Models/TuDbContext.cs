using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace ALMACENHV.Models
{
    public class TuDbContext : DbContext
    {
        public TuDbContext(DbContextOptions<TuDbContext> options) : base(options)
        {
        }

        // Define tus DbSets aquí
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Apunte> Apuntes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<RegistroIngresoDetalle> RegistroIngresoDetalles { get; set; }
        public DbSet<RegistroSalidaDetalle> RegistroSalidaDetalles { get; set; }
        public DbSet<RegistroEntrada> RegistroEntradas { get; set; }
        public DbSet<RegistroSalida> RegistroSalidas { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Seccion> Secciones { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<RegistroIngresoFoto> RegistroIngresoFotos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para la entidad Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(e => e.Precio)
                      .HasColumnType("decimal(18, 2)"); // Especifica el tipo de columna
            });

            // Configuración para otras entidades
            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.Property(e => e.NombreCargo)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.Property(e => e.NombreRol)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.NombreUsuario)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Password)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<RegistroEntrada>(entity =>
            {
                entity.Property(e => e.Fecha)
                      .IsRequired();
            });

            modelBuilder.Entity<RegistroSalida>(entity =>
            {
                entity.Property(e => e.Fecha)
                      .IsRequired();
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.Property(e => e.NombreProveedor)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Seccion>(entity =>
            {
                entity.Property(e => e.NombreSeccion)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Ubicacion>(entity =>
            {
                entity.Property(e => e.NombreUbicacion)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Configuración para otras entidades
        }
    }
}