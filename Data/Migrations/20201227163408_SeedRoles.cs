using Microsoft.EntityFrameworkCore.Migrations;

namespace whiskyserverapp.Data.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "abfd3a33-8a3b-46fd-8726-8601b31c4f2c", "6c57caf7-611b-409c-95a1-58b1b7451616", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1d89fbbc-b6fe-4cb3-8d1d-f6b3d93a8612", "16d4a15e-146c-4502-a532-c6f54bdeb753", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d89fbbc-b6fe-4cb3-8d1d-f6b3d93a8612");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abfd3a33-8a3b-46fd-8726-8601b31c4f2c");
        }
    }
}
