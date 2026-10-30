using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class TINdataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TIN",
                table: "Persons",
                newName: "TaxIdentificationNumber");

            migrationBuilder.AlterColumn<string>(
                name: "TaxIdentificationNumber",
                table: "Persons",
                type: "varchar(15)",
                nullable: true,
                defaultValue: "ABC45679",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0001-4b22-8b01-000000000001"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0002-4b22-8b01-000000000002"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0003-4b22-8b01-000000000003"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0004-4b22-8b01-000000000004"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0005-4b22-8b01-000000000005"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0006-4b22-8b01-000000000006"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0007-4b22-8b01-000000000007"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0008-4b22-8b01-000000000008"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0009-4b22-8b01-000000000009"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0010-4b22-8b01-000000000010"),
                column: "TaxIdentificationNumber",
                value: "ABC45679");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxIdentificationNumber",
                table: "Persons",
                newName: "TIN");

            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldNullable: true,
                oldDefaultValue: "ABC45679");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0001-4b22-8b01-000000000001"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0002-4b22-8b01-000000000002"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0003-4b22-8b01-000000000003"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0004-4b22-8b01-000000000004"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0005-4b22-8b01-000000000005"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0006-4b22-8b01-000000000006"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0007-4b22-8b01-000000000007"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0008-4b22-8b01-000000000008"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0009-4b22-8b01-000000000009"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c2b6d1ef-0010-4b22-8b01-000000000010"),
                column: "TIN",
                value: null);
        }
    }
}
