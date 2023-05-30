using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
//Estas clases se crean solas despues de hacer la migracion, despues de hacer la clase en los modelos
// y despues de crear las clases de los DTO y haber realizado el mapping 
namespace MagicVillaApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroVillaTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumeroVillas",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeroVillas", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_NumeroVillas_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 5, 29, 17, 43, 50, 663, DateTimeKind.Local).AddTicks(3817), new DateTime(2023, 5, 29, 17, 43, 50, 663, DateTimeKind.Local).AddTicks(3806) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 5, 29, 17, 43, 50, 663, DateTimeKind.Local).AddTicks(3820), new DateTime(2023, 5, 29, 17, 43, 50, 663, DateTimeKind.Local).AddTicks(3819) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVillas_VillaId",
                table: "NumeroVillas",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumeroVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 5, 28, 21, 35, 52, 895, DateTimeKind.Local).AddTicks(4535), new DateTime(2023, 5, 28, 21, 35, 52, 895, DateTimeKind.Local).AddTicks(4525) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 5, 28, 21, 35, 52, 895, DateTimeKind.Local).AddTicks(4537), new DateTime(2023, 5, 28, 21, 35, 52, 895, DateTimeKind.Local).AddTicks(4537) });
        }
    }
}
