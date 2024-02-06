using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedVillaToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "CreatedDate", "Description", "ImageUrl", "Name", "Occupancy", "Squarefeet", "UpdatedDate", "price" },
                values: new object[,]
                {
                    { 1, null, "Description of Royal Villa", "https://placehold.co/600x400", "Royal Villa", 4, 550, null, 200.0 },
                    { 2, null, "Description of Premium Pool Villa", "https://placehold.co/600x400", "Premium Pool Villa", 4, 580, null, 300.0 },
                    { 3, null, "Description of Luxury Pool Villa", "https://placehold.co/600x400", "Luxury Pool Villa", 3, 720, null, 400.0 }
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
        }
    }
}
