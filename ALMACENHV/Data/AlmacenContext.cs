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
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Seccion> Secciones { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<RegistroEntrada> RegistroEntradas { get; set; }
        public DbSet<RegistroSalida> RegistroSalidas { get; set; }
        public DbSet<RegistroEntradaDetalle> RegistroEntradaDetalles { get; set; }
        public DbSet<RegistroSalidaDetalle> RegistroSalidaDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolID)
                .OnDelete(DeleteBehavior.Restrict);

            // Rol
            modelBuilder.Entity<Rol>()
                .HasIndex(r => r.Nombre)
                .IsUnique();

            // Producto
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Codigo)
                .IsUnique();

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Ubicacion)
                .WithMany(u => u.Productos)
                .HasForeignKey(p => p.UbicacionID)
                .OnDelete(DeleteBehavior.Restrict);

            // Ubicacion
            modelBuilder.Entity<Ubicacion>()
                .HasOne(u => u.Seccion)
                .WithMany(s => s.Ubicaciones)
                .HasForeignKey(u => u.SeccionID)
                .OnDelete(DeleteBehavior.Restrict);

            // Seccion
            modelBuilder.Entity<Seccion>()
                .HasIndex(s => s.Nombre)
                .IsUnique();

            // RegistroEntrada
            modelBuilder.Entity<RegistroEntrada>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            // RegistroEntradaDetalle
            modelBuilder.Entity<RegistroEntradaDetalle>()
                .HasOne(d => d.RegistroEntrada)
                .WithMany(r => r.Detalles)
                .HasForeignKey(d => d.RegistroEntradaID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RegistroEntradaDetalle>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoID)
                .OnDelete(DeleteBehavior.Restrict);

            // RegistroSalida
            modelBuilder.Entity<RegistroSalida>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            // RegistroSalidaDetalle
            modelBuilder.Entity<RegistroSalidaDetalle>()
                .HasOne(d => d.RegistroSalida)
                .WithMany(r => r.Detalles)
                .HasForeignKey(d => d.RegistroSalidaID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RegistroSalidaDetalle>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoID)
                .OnDelete(DeleteBehavior.Restrict);

            // Eventos
            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
