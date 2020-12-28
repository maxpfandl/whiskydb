using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace whiskydb.Data.Migrations
{
    public partial class WhiskySheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d89fbbc-b6fe-4cb3-8d1d-f6b3d93a8612");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abfd3a33-8a3b-46fd-8726-8601b31c4f2c");

            migrationBuilder.CreateTable(
                name: "Whiskys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Age = table.Column<int>(type: "INTEGER", nullable: true),
                    Alc = table.Column<decimal>(type: "TEXT", nullable: true),
                    Distilled = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Botteled = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Aquired = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Opened = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastPour = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PourDates = table.Column<string>(type: "TEXT", nullable: true),
                    Pours = table.Column<int>(type: "INTEGER", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    Nose = table.Column<string>(type: "TEXT", nullable: true),
                    Palate = table.Column<string>(type: "TEXT", nullable: true),
                    Finish = table.Column<string>(type: "TEXT", nullable: true),
                    Rating = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrlTh = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Finished = table.Column<bool>(type: "INTEGER", nullable: false),
                    UnChillfiltered = table.Column<bool>(type: "INTEGER", nullable: false),
                    ArtColor = table.Column<bool>(type: "INTEGER", nullable: false),
                    CaskStrength = table.Column<bool>(type: "INTEGER", nullable: false),
                    SingleCask = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Whiskys", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "38a32d69-000a-467c-9e4a-9ee472aa89eb", "21ff2b1b-877c-4d10-8223-87101e0ad9f3", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ac665fbc-ff44-4266-8162-be5a2253d0c3", "9284d4aa-3c5c-4e88-801e-5378519db016", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Whiskys");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38a32d69-000a-467c-9e4a-9ee472aa89eb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac665fbc-ff44-4266-8162-be5a2253d0c3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "abfd3a33-8a3b-46fd-8726-8601b31c4f2c", "6c57caf7-611b-409c-95a1-58b1b7451616", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1d89fbbc-b6fe-4cb3-8d1d-f6b3d93a8612", "16d4a15e-146c-4502-a532-c6f54bdeb753", "Admin", "ADMIN" });
        }
    }
}
