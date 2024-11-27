using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ALMACENHV.Migrations
{
    /// <inheritdoc />
    public partial class CapturePendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    CargoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.CargoID);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    ProveedorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProveedor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.ProveedorID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolID);
                });

            migrationBuilder.CreateTable(
                name: "Secciones",
                columns: table => new
                {
                    SeccionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreSeccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secciones", x => x.SeccionID);
                });

            migrationBuilder.CreateTable(
                name: "Ubicaciones",
                columns: table => new
                {
                    UbicacionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUbicacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicaciones", x => x.UbicacionID);
                });

            // Verifica si la tabla Usuarios ya existe antes de intentar crearla
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
                BEGIN
                    CREATE TABLE [Usuarios] (
                        [UsuarioID] int NOT NULL IDENTITY,
                        [NombreUsuario] nvarchar(100) NOT NULL,
                        [Email] nvarchar(100) NOT NULL,
                        [Password] nvarchar(100) NOT NULL,
                        [RolID] int NOT NULL,
                        [CargoID] int NOT NULL,
                        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([UsuarioID]),
                        CONSTRAINT [FK_Usuarios_Cargos_CargoID] FOREIGN KEY ([CargoID]) REFERENCES [Cargos] ([CargoID]) ON DELETE CASCADE,
                        CONSTRAINT [FK_Usuarios_Roles_RolID] FOREIGN KEY ([RolID]) REFERENCES [Roles] ([RolID]) ON DELETE CASCADE
                    );
                END
            ");

            // Verifica si la tabla Productos ya existe antes de intentar crearla
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Productos')
                BEGIN
                    CREATE TABLE [Productos] (
                        [ProductoID] int NOT NULL IDENTITY,
                        [NombreProducto] nvarchar(max) NOT NULL,
                        [Descripcion] nvarchar(max) NOT NULL,
                        [Precio] decimal(18,2) NOT NULL,
                        [Stock] int NOT NULL,
                        [SeccionID] int NOT NULL,
                        CONSTRAINT [PK_Productos] PRIMARY KEY ([ProductoID]),
                        CONSTRAINT [FK_Productos_Secciones_SeccionID] FOREIGN KEY ([SeccionID]) REFERENCES [Secciones] ([SeccionID]) ON DELETE CASCADE
                    );
                END
            ");

            // Verifica si la tabla Apuntes ya existe antes de intentar crearla
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Apuntes')
                BEGIN
                    CREATE TABLE [Apuntes] (
                        [ApunteID] int NOT NULL IDENTITY,
                        [Descripcion] nvarchar(max) NOT NULL,
                        [Fecha] datetime2 NOT NULL,
                        [UsuarioID] int NOT NULL,
                        CONSTRAINT [PK_Apuntes] PRIMARY KEY ([ApunteID]),
                        CONSTRAINT [FK_Apuntes_Usuarios_UsuarioID] FOREIGN KEY ([UsuarioID]) REFERENCES [Usuarios] ([UsuarioID]) ON DELETE CASCADE
                    );
                END
            ");

            // Verifica si la tabla Eventos ya existe antes de intentar crearla
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Eventos')
                BEGIN
                    CREATE TABLE [Eventos] (
                        [EventoID] int NOT NULL IDENTITY,
                        [NombreEvento] nvarchar(max) NOT NULL,
                        [Descripcion] nvarchar(max) NOT NULL,
                        [Fecha] datetime2 NOT NULL,
                        [UsuarioID] int NOT NULL,
                        CONSTRAINT [PK_Eventos] PRIMARY KEY ([EventoID]),
                        CONSTRAINT [FK_Eventos_Usuarios_UsuarioID] FOREIGN KEY ([UsuarioID]) REFERENCES [Usuarios] ([UsuarioID]) ON DELETE CASCADE
                    );
                END
            ");

            migrationBuilder.CreateTable(
                name: "RegistroEntradas",
                columns: table => new
                {
                    RegistroEntradaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    ProveedorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroEntradas", x => x.RegistroEntradaID);
                    table.ForeignKey(
                        name: "FK_RegistroEntradas_Proveedores_ProveedorID",
                        column: x => x.ProveedorID,
                        principalTable: "Proveedores",
                        principalColumn: "ProveedorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistroEntradas_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistroSalidas",
                columns: table => new
                {
                    RegistroSalidaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    UbicacionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroSalidas", x => x.RegistroSalidaID);
                    table.ForeignKey(
                        name: "FK_RegistroSalidas_Ubicaciones_UbicacionID",
                        column: x => x.UbicacionID,
                        principalTable: "Ubicaciones",
                        principalColumn: "UbicacionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistroSalidas_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistroIngresoDetalles",
                columns: table => new
                {
                    RegistroIngresoDetalleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistroIngresoID = table.Column<int>(type: "int", nullable: false),
                    ProductoID = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroIngresoDetalles", x => x.RegistroIngresoDetalleID);
                    table.ForeignKey(
                        name: "FK_RegistroIngresoDetalles_Productos_ProductoID",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistroIngresoDetalles_RegistroEntradas_RegistroIngresoID",
                        column: x => x.RegistroIngresoID,
                        principalTable: "RegistroEntradas",
                        principalColumn: "RegistroEntradaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistroIngresoFotos",
                columns: table => new
                {
                    RegistroIngresoFotoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistroIngresoID = table.Column<int>(type: "int", nullable: false),
                    FotoURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroIngresoFotos", x => x.RegistroIngresoFotoID);
                    table.ForeignKey(
                        name: "FK_RegistroIngresoFotos_RegistroEntradas_RegistroIngresoID",
                        column: x => x.RegistroIngresoID,
                        principalTable: "RegistroEntradas",
                        principalColumn: "RegistroEntradaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistroSalidaDetalles",
                columns: table => new
                {
                    RegistroSalidaDetalleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistroSalidaID = table.Column<int>(type: "int", nullable: false),
                    ProductoID = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroSalidaDetalles", x => x.RegistroSalidaDetalleID);
                    table.ForeignKey(
                        name: "FK_RegistroSalidaDetalles_Productos_ProductoID",
                        column: x => x.ProductoID,
                        principalTable: "Productos",
                        principalColumn: "ProductoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistroSalidaDetalles_RegistroSalidas_RegistroSalidaID",
                        column: x => x.RegistroSalidaID,
                        principalTable: "RegistroSalidas",
                        principalColumn: "RegistroSalidaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apuntes_UsuarioID",
                table: "Apuntes",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioID",
                table: "Eventos",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_SeccionID",
                table: "Productos",
                column: "SeccionID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroEntradas_ProveedorID",
                table: "RegistroEntradas",
                column: "ProveedorID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroEntradas_UsuarioID",
                table: "RegistroEntradas",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroIngresoDetalles_ProductoID",
                table: "RegistroIngresoDetalles",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroIngresoDetalles_RegistroIngresoID",
                table: "RegistroIngresoDetalles",
                column: "RegistroIngresoID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroIngresoFotos_RegistroIngresoID",
                table: "RegistroIngresoFotos",
                column: "RegistroIngresoID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroSalidaDetalles_ProductoID",
                table: "RegistroSalidaDetalles",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroSalidaDetalles_RegistroSalidaID",
                table: "RegistroSalidaDetalles",
                column: "RegistroSalidaID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroSalidas_UbicacionID",
                table: "RegistroSalidas",
                column: "UbicacionID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroSalidas_UsuarioID",
                table: "RegistroSalidas",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CargoID",
                table: "Usuarios",
                column: "CargoID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolID",
                table: "Usuarios",
                column: "RolID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apuntes");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "RegistroIngresoDetalles");

            migrationBuilder.DropTable(
                name: "RegistroIngresoFotos");

            migrationBuilder.DropTable(
                name: "RegistroSalidaDetalles");

            migrationBuilder.DropTable(
                name: "RegistroEntradas");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "RegistroSalidas");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Secciones");

            migrationBuilder.DropTable(
                name: "Ubicaciones");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Cargos");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}