using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Data
{
    public class AlmacenContext : DbContext
    {
        public AlmacenContext(DbContextOptions<AlmacenContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Seccion> Secciones { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<RegistroEntrada> RegistroEntradas { get; set; }
        public DbSet<RegistroSalida> RegistroSalidas { get; set; }
        public DbSet<RegistroIngresoDetalle> RegistroIngresoDetalles { get; set; }
        public DbSet<RegistroSalidaDetalle> RegistroSalidaDetalles { get; set; }
        public DbSet<RegistroIngresoFoto> RegistroIngresoFotos { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Apunte> Apuntes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cargo>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<Cargo>()
                .Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Producto>()
                .Property(p => p.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreUsuario)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => new { u.RolID, u.CargoID });

            modelBuilder.Entity<Ubicacion>()
                .HasIndex(u => u.Codigo)
                .IsUnique();

            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.CodigoInterno)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Cargo)
                .WithMany(c => c.Usuarios)
                .HasForeignKey(u => u.CargoID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.RegistroEntradas)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.RegistroSalidas)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.NombreUsuario)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Password)
                .IsRequired();

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Seccion)
                .WithMany()
                .HasForeignKey(p => p.SeccionID);

            modelBuilder.Entity<RegistroEntrada>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioID);

            modelBuilder.Entity<RegistroEntrada>()
                .HasOne(r => r.Proveedor)
                .WithMany()
                .HasForeignKey(r => r.ProveedorID);

            modelBuilder.Entity<RegistroSalida>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioID);

            modelBuilder.Entity<RegistroSalida>()
                .HasOne(r => r.Ubicacion)
                .WithMany()
                .HasForeignKey(r => r.UbicacionID);

            modelBuilder.Entity<RegistroIngresoDetalle>()
                .HasOne(d => d.RegistroIngreso)
                .WithMany()
                .HasForeignKey(d => d.RegistroIngresoID);

            modelBuilder.Entity<RegistroIngresoDetalle>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoID);

            modelBuilder.Entity<RegistroSalidaDetalle>()
                .HasOne(d => d.RegistroSalida)
                .WithMany()
                .HasForeignKey(d => d.RegistroSalidaID);

            modelBuilder.Entity<RegistroSalidaDetalle>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoID);

            modelBuilder.Entity<RegistroIngresoFoto>()
                .HasOne(f => f.RegistroIngreso)
                .WithMany()
                .HasForeignKey(f => f.RegistroIngresoID);

            modelBuilder.Entity<Rol>().HasData(
                new Rol { RolID = 1, Nombre = "Administrador", Descripcion = "Administrador del sistema" },
                new Rol { RolID = 2, Nombre = "Usuario", Descripcion = "Usuario estándar" }
            );

            modelBuilder.Entity<Cargo>().HasData(
                new Cargo { CargoID = 1, Nombre = "Gerente", Descripcion = "Gerente de almacén" },
                new Cargo { CargoID = 2, Nombre = "Operador", Descripcion = "Operador de almacén" }
            );
        }
    }
}
