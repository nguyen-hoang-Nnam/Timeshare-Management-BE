using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeshareManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatetablebooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRequests_Timeshares_timeshareId",
                table: "BookingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Timeshares_Places_placeId",
                table: "Timeshares");

            migrationBuilder.DropForeignKey(
                name: "FK_Timeshares_TimesharesStatus_timeshareStatusId",
                table: "Timeshares");

            migrationBuilder.AddColumn<int>(
                name: "timeshareStatusId",
                table: "BookingRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingRequests_timeshareStatusId",
                table: "BookingRequests",
                column: "timeshareStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRequests_TimesharesStatus_timeshareStatusId",
                table: "BookingRequests",
                column: "timeshareStatusId",
                principalTable: "TimesharesStatus",
                principalColumn: "timeshareStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRequests_Timeshares_timeshareId",
                table: "BookingRequests",
                column: "timeshareId",
                principalTable: "Timeshares",
                principalColumn: "timeshareId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Timeshares_Places_placeId",
                table: "Timeshares",
                column: "placeId",
                principalTable: "Places",
                principalColumn: "placeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Timeshares_TimesharesStatus_timeshareStatusId",
                table: "Timeshares",
                column: "timeshareStatusId",
                principalTable: "TimesharesStatus",
                principalColumn: "timeshareStatusId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRequests_TimesharesStatus_timeshareStatusId",
                table: "BookingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingRequests_Timeshares_timeshareId",
                table: "BookingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Timeshares_Places_placeId",
                table: "Timeshares");

            migrationBuilder.DropForeignKey(
                name: "FK_Timeshares_TimesharesStatus_timeshareStatusId",
                table: "Timeshares");

            migrationBuilder.DropIndex(
                name: "IX_BookingRequests_timeshareStatusId",
                table: "BookingRequests");

            migrationBuilder.DropColumn(
                name: "timeshareStatusId",
                table: "BookingRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRequests_Timeshares_timeshareId",
                table: "BookingRequests",
                column: "timeshareId",
                principalTable: "Timeshares",
                principalColumn: "timeshareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timeshares_Places_placeId",
                table: "Timeshares",
                column: "placeId",
                principalTable: "Places",
                principalColumn: "placeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timeshares_TimesharesStatus_timeshareStatusId",
                table: "Timeshares",
                column: "timeshareStatusId",
                principalTable: "TimesharesStatus",
                principalColumn: "timeshareStatusId");
        }
    }
}
