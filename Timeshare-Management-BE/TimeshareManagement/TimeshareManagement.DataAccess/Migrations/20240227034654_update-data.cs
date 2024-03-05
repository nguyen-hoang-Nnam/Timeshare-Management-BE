using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeshareManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomDetail_RoomAmenities_roomAmenitiesId",
                table: "RoomDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesharesDetail_Rooms_roomID",
                table: "TimesharesDetail");

            migrationBuilder.DropIndex(
                name: "IX_TimesharesDetail_roomID",
                table: "TimesharesDetail");

            migrationBuilder.DropIndex(
                name: "IX_RoomDetail_roomAmenitiesId",
                table: "RoomDetail");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TimesharesStatus");

            migrationBuilder.DropColumn(
                name: "roomID",
                table: "TimesharesDetail");

            migrationBuilder.DropColumn(
                name: "roomAmenitiesId",
                table: "RoomDetail");

            migrationBuilder.AddColumn<string>(
                name: "timeshareStatusName",
                table: "TimesharesStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "timeshareDetailId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "roomDetailId",
                table: "RoomAmenities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_timeshareDetailId",
                table: "Rooms",
                column: "timeshareDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAmenities_roomDetailId",
                table: "RoomAmenities",
                column: "roomDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAmenities_RoomDetail_roomDetailId",
                table: "RoomAmenities",
                column: "roomDetailId",
                principalTable: "RoomDetail",
                principalColumn: "roomDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_TimesharesDetail_timeshareDetailId",
                table: "Rooms",
                column: "timeshareDetailId",
                principalTable: "TimesharesDetail",
                principalColumn: "timeshareDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAmenities_RoomDetail_roomDetailId",
                table: "RoomAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_TimesharesDetail_timeshareDetailId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_timeshareDetailId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_RoomAmenities_roomDetailId",
                table: "RoomAmenities");

            migrationBuilder.DropColumn(
                name: "timeshareStatusName",
                table: "TimesharesStatus");

            migrationBuilder.DropColumn(
                name: "timeshareDetailId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "roomDetailId",
                table: "RoomAmenities");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TimesharesStatus",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "roomID",
                table: "TimesharesDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "roomAmenitiesId",
                table: "RoomDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimesharesDetail_roomID",
                table: "TimesharesDetail",
                column: "roomID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomDetail_roomAmenitiesId",
                table: "RoomDetail",
                column: "roomAmenitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomDetail_RoomAmenities_roomAmenitiesId",
                table: "RoomDetail",
                column: "roomAmenitiesId",
                principalTable: "RoomAmenities",
                principalColumn: "roomAmenitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimesharesDetail_Rooms_roomID",
                table: "TimesharesDetail",
                column: "roomID",
                principalTable: "Rooms",
                principalColumn: "roomID");
        }
    }
}
