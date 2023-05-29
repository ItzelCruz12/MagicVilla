using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaApi.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 5, 28, 21, 21, 14, 509, DateTimeKind.Local).AddTicks(6840), new DateTime(2023, 5, 28, 21, 21, 14, 509, DateTimeKind.Local).AddTicks(6827) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 5, 28, 21, 21, 14, 509, DateTimeKind.Local).AddTicks(6844), new DateTime(2023, 5, 28, 21, 21, 14, 509, DateTimeKind.Local).AddTicks(6843) });
        }
    }
}
