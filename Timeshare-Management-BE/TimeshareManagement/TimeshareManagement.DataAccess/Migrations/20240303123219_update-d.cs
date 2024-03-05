using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeshareManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimesharesDetail_Timeshares_timeshareId",
                table: "TimesharesDetail");

            migrationBuilder.DropIndex(
                name: "IX_TimesharesDetail_timeshareId",
                table: "TimesharesDetail");

            migrationBuilder.DropColumn(
                name: "timeshareId",
                table: "TimesharesDetail");

            migrationBuilder.AddColumn<string>(
                name: "Detail",
                table: "Timeshares",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Detail",
                table: "Timeshares");

            migrationBuilder.AddColumn<int>(
                name: "timeshareId",
                table: "TimesharesDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimesharesDetail_timeshareId",
                table: "TimesharesDetail",
                column: "timeshareId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimesharesDetail_Timeshares_timeshareId",
                table: "TimesharesDetail",
                column: "timeshareId",
                principalTable: "Timeshares",
                principalColumn: "timeshareId");
        }
    }
}
