using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeshareManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateroomdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_TimesharesDetail_timeshareDetailId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "timeshareDetailId",
                table: "Rooms",
                newName: "timeshareId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_timeshareDetailId",
                table: "Rooms",
                newName: "IX_Rooms_timeshareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Timeshares_timeshareId",
                table: "Rooms",
                column: "timeshareId",
                principalTable: "Timeshares",
                principalColumn: "timeshareId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Timeshares_timeshareId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "timeshareId",
                table: "Rooms",
                newName: "timeshareDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_timeshareId",
                table: "Rooms",
                newName: "IX_Rooms_timeshareDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_TimesharesDetail_timeshareDetailId",
                table: "Rooms",
                column: "timeshareDetailId",
                principalTable: "TimesharesDetail",
                principalColumn: "timeshareDetailId");
        }
    }
}
