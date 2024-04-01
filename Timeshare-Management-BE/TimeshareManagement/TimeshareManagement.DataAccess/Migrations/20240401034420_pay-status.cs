using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeshareManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class paystatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "timeshareStatusId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_timeshareStatusId",
                table: "Payments",
                column: "timeshareStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_TimesharesStatus_timeshareStatusId",
                table: "Payments",
                column: "timeshareStatusId",
                principalTable: "TimesharesStatus",
                principalColumn: "timeshareStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_TimesharesStatus_timeshareStatusId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_timeshareStatusId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "timeshareStatusId",
                table: "Payments");
        }
    }
}
