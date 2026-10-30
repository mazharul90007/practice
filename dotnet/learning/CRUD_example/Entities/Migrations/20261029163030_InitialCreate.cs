using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("b1a5c0de-0001-4a11-9a01-000000000001"), "USA" },
                    { new Guid("b1a5c0de-0002-4a11-9a01-000000000002"), "Canada" },
                    { new Guid("b1a5c0de-0003-4a11-9a01-000000000003"), "United Kingdom" },
                    { new Guid("b1a5c0de-0004-4a11-9a01-000000000004"), "India" },
                    { new Guid("b1a5c0de-0005-4a11-9a01-000000000005"), "Bangladesh" },
                    { new Guid("b1a5c0de-0006-4a11-9a01-000000000006"), "Australia" },
                    { new Guid("b1a5c0de-0007-4a11-9a01-000000000007"), "Germany" },
                    { new Guid("b1a5c0de-0008-4a11-9a01-000000000008"), "France" },
                    { new Guid("b1a5c0de-0009-4a11-9a01-000000000009"), "Japan" },
                    { new Guid("b1a5c0de-0010-4a11-9a01-000000000010"), "Brazil" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("c2b6d1ef-0001-4b22-8b01-000000000001"), "120 Main Street, New York", new Guid("b1a5c0de-0001-4a11-9a01-000000000001"), new DateTime(1990, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "james.carter@example.com", "Male", "James Carter", true },
                    { new Guid("c2b6d1ef-0002-4b22-8b01-000000000002"), "45 Maple Avenue, Toronto", new Guid("b1a5c0de-0002-4a11-9a01-000000000002"), new DateTime(1988, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "emily.tremblay@example.com", "Female", "Emily Tremblay", false },
                    { new Guid("c2b6d1ef-0003-4b22-8b01-000000000003"), "9 Baker Street, London", new Guid("b1a5c0de-0003-4a11-9a01-000000000003"), new DateTime(1995, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "oliver.smith@example.com", "Male", "Oliver Smith", true },
                    { new Guid("c2b6d1ef-0004-4b22-8b01-000000000004"), "22 MG Road, Bengaluru", new Guid("b1a5c0de-0004-4a11-9a01-000000000004"), new DateTime(1992, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "priya.sharma@example.com", "Female", "Priya Sharma", true },
                    { new Guid("c2b6d1ef-0005-4b22-8b01-000000000005"), "77 Gulshan Avenue, Dhaka", new Guid("b1a5c0de-0005-4a11-9a01-000000000005"), new DateTime(1985, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "rahim.uddin@example.com", "Male", "Rahim Uddin", false },
                    { new Guid("c2b6d1ef-0006-4b22-8b01-000000000006"), "15 Beach Road, Sydney", new Guid("b1a5c0de-0006-4a11-9a01-000000000006"), new DateTime(1998, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "chloe.wilson@example.com", "Female", "Chloe Wilson", true },
                    { new Guid("c2b6d1ef-0007-4b22-8b01-000000000007"), "3 Berliner Strasse, Berlin", new Guid("b1a5c0de-0007-4a11-9a01-000000000007"), new DateTime(1991, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "lukas.schneider@example.com", "Male", "Lukas Schneider", false },
                    { new Guid("c2b6d1ef-0008-4b22-8b01-000000000008"), "18 Rue de Rivoli, Paris", new Guid("b1a5c0de-0008-4a11-9a01-000000000008"), new DateTime(1994, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "camille.dubois@example.com", "Female", "Camille Dubois", true },
                    { new Guid("c2b6d1ef-0009-4b22-8b01-000000000009"), "6 Shibuya, Tokyo", new Guid("b1a5c0de-0009-4a11-9a01-000000000009"), new DateTime(1989, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "haruto.tanaka@example.com", "Other", "Haruto Tanaka", false },
                    { new Guid("c2b6d1ef-0010-4b22-8b01-000000000010"), "50 Avenida Paulista, Sao Paulo", new Guid("b1a5c0de-0010-4a11-9a01-000000000010"), new DateTime(1996, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "sofia.almeida@example.com", "Female", "Sofia Almeida", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
