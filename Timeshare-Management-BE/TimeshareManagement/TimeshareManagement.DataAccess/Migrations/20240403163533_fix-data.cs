using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeshareManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "timeshareName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "userEmail",
                table: "Payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentName",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "timeshareName",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "userEmail",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
