using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeshareManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class datetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicDate",
                table: "Timeshares",
                newName: "dateTo");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateFrom",
                table: "Timeshares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateFrom",
                table: "Timeshares");

            migrationBuilder.RenameColumn(
                name: "dateTo",
                table: "Timeshares",
                newName: "PublicDate");
        }
    }
}
