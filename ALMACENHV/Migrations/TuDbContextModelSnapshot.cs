// <auto-generated />
using System;
using ALMACENHV.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ALMACENHV.Data;

#nullable disable

namespace ALMACENHV.Migrations
{
    [DbContext(typeof(AlmacenContext))]
    partial class AlmacenContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ALMACENHV.Models.Apunte", b =>
                {
                    b.Property<int>("ApunteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApunteID"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("int");

                    b.HasKey("ApunteID");

                    b.HasIndex("UsuarioID");

                    b.ToTable("Apuntes");
                });

            modelBuilder.Entity("ALMACENHV.Models.Cargo", b =>
                {
                    b.Property<int>("CargoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CargoID"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreCargo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CargoID");

                    b.ToTable("Cargos");
                });

            modelBuilder.Entity("ALMACENHV.Models.Evento", b =>
                {
                    b.Property<int>("EventoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventoID"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("NombreEvento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("int");

                    b.HasKey("EventoID");

                    b.HasIndex("UsuarioID");

                    b.ToTable("Eventos");
                });

            modelBuilder.Entity("ALMACENHV.Models.Producto", b =>
                {
                    b.Property<int>("ProductoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductoID"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreProducto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("SeccionID")
                        .HasColumnType("int");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("ProductoID");

                    b.HasIndex("SeccionID");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("ALMACENHV.Models.Proveedor", b =>
                {
                    b.Property<int>("ProveedorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProveedorID"));

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreProveedor")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProveedorID");

                    b.ToTable("Proveedores");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroEntrada", b =>
                {
                    b.Property<int>("RegistroEntradaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegistroEntradaID"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProveedorID")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("int");

                    b.HasKey("RegistroEntradaID");

                    b.HasIndex("ProveedorID");

                    b.HasIndex("UsuarioID");

                    b.ToTable("RegistroEntradas");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroIngresoDetalle", b =>
                {
                    b.Property<int>("RegistroIngresoDetalleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegistroIngresoDetalleID"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("ProductoID")
                        .HasColumnType("int");

                    b.Property<int>("RegistroIngresoID")
                        .HasColumnType("int");

                    b.HasKey("RegistroIngresoDetalleID");

                    b.HasIndex("ProductoID");

                    b.HasIndex("RegistroIngresoID");

                    b.ToTable("RegistroIngresoDetalles");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroIngresoFoto", b =>
                {
                    b.Property<int>("RegistroIngresoFotoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegistroIngresoFotoID"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FotoURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RegistroIngresoID")
                        .HasColumnType("int");

                    b.HasKey("RegistroIngresoFotoID");

                    b.HasIndex("RegistroIngresoID");

                    b.ToTable("RegistroIngresoFotos");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroSalida", b =>
                {
                    b.Property<int>("RegistroSalidaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegistroSalidaID"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("UbicacionID")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("int");

                    b.HasKey("RegistroSalidaID");

                    b.HasIndex("UbicacionID");

                    b.HasIndex("UsuarioID");

                    b.ToTable("RegistroSalidas");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroSalidaDetalle", b =>
                {
                    b.Property<int>("RegistroSalidaDetalleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegistroSalidaDetalleID"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("ProductoID")
                        .HasColumnType("int");

                    b.Property<int>("RegistroSalidaID")
                        .HasColumnType("int");

                    b.HasKey("RegistroSalidaDetalleID");

                    b.HasIndex("ProductoID");

                    b.HasIndex("RegistroSalidaID");

                    b.ToTable("RegistroSalidaDetalles");
                });

            modelBuilder.Entity("ALMACENHV.Models.Rol", b =>
                {
                    b.Property<int>("RolID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RolID"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreRol")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("RolID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ALMACENHV.Models.Seccion", b =>
                {
                    b.Property<int>("SeccionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SeccionID"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreSeccion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("SeccionID");

                    b.ToTable("Secciones");
                });

            modelBuilder.Entity("ALMACENHV.Models.Ubicacion", b =>
                {
                    b.Property<int>("UbicacionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UbicacionID"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreUbicacion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UbicacionID");

                    b.ToTable("Ubicaciones");
                });

            modelBuilder.Entity("ALMACENHV.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioID"));

                    b.Property<int>("CargoID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RolID")
                        .HasColumnType("int");

                    b.HasKey("UsuarioID");

                    b.HasIndex("CargoID");

                    b.HasIndex("RolID");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ALMACENHV.Models.Apunte", b =>
                {
                    b.HasOne("ALMACENHV.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ALMACENHV.Models.Evento", b =>
                {
                    b.HasOne("ALMACENHV.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ALMACENHV.Models.Producto", b =>
                {
                    b.HasOne("ALMACENHV.Models.Seccion", "Seccion")
                        .WithMany()
                        .HasForeignKey("SeccionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seccion");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroEntrada", b =>
                {
                    b.HasOne("ALMACENHV.Models.Proveedor", "Proveedor")
                        .WithMany()
                        .HasForeignKey("ProveedorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ALMACENHV.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Proveedor");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroIngresoDetalle", b =>
                {
                    b.HasOne("ALMACENHV.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ALMACENHV.Models.RegistroEntrada", "RegistroIngreso")
                        .WithMany()
                        .HasForeignKey("RegistroIngresoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");

                    b.Navigation("RegistroIngreso");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroIngresoFoto", b =>
                {
                    b.HasOne("ALMACENHV.Models.RegistroEntrada", "RegistroIngreso")
                        .WithMany()
                        .HasForeignKey("RegistroIngresoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RegistroIngreso");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroSalida", b =>
                {
                    b.HasOne("ALMACENHV.Models.Ubicacion", "Ubicacion")
                        .WithMany()
                        .HasForeignKey("UbicacionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ALMACENHV.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ubicacion");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ALMACENHV.Models.RegistroSalidaDetalle", b =>
                {
                    b.HasOne("ALMACENHV.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ALMACENHV.Models.RegistroSalida", "RegistroSalida")
                        .WithMany()
                        .HasForeignKey("RegistroSalidaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");

                    b.Navigation("RegistroSalida");
                });

            modelBuilder.Entity("ALMACENHV.Models.Usuario", b =>
                {
                    b.HasOne("ALMACENHV.Models.Cargo", "Cargo")
                        .WithMany()
                        .HasForeignKey("CargoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ALMACENHV.Models.Rol", "Rol")
                        .WithMany()
                        .HasForeignKey("RolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cargo");

                    b.Navigation("Rol");
                });
#pragma warning restore 612, 618
        }
    }
}
