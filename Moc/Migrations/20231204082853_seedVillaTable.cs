using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MOC1.Migrations
{
    /// <inheritdoc />
    public partial class seedVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "full", new DateTime(2023, 12, 4, 15, 28, 53, 83, DateTimeKind.Local).AddTicks(5581), "HaLong Beach", "https", "Beach view", 500, 4.0, 100, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "half", new DateTime(2023, 12, 4, 15, 28, 53, 83, DateTimeKind.Local).AddTicks(5594), "ThienVan Mount", "https", "Mountain view", 1000, 3.5, 200, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "full", new DateTime(2023, 12, 4, 15, 28, 53, 83, DateTimeKind.Local).AddTicks(5596), "DoSon Beach", "https", "ABC resort", 400, 5.0, 150, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "temp", new DateTime(2023, 12, 4, 15, 28, 53, 83, DateTimeKind.Local).AddTicks(5598), "DongDo Lake", "https", "Lake view", 100, 4.0, 75, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
