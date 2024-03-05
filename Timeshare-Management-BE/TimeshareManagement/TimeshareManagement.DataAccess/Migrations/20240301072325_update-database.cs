using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeshareManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRequests_Rooms_roomID",
                table: "BookingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomAmenities_RoomDetail_roomDetailId",
                table: "RoomAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_Timeshares_AspNetUsers_Id",
                table: "Timeshares");

            migrationBuilder.DropTable(
                name: "RoomDetail");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Timeshares_Id",
                table: "Timeshares");

            migrationBuilder.DropIndex(
                name: "IX_RoomAmenities_roomDetailId",
                table: "RoomAmenities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Timeshares");

            migrationBuilder.DropColumn(
                name: "roomDetailId",
                table: "RoomAmenities");

            migrationBuilder.RenameColumn(
                name: "roomID",
                table: "BookingRequests",
                newName: "timeshareId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingRequests_roomID",
                table: "BookingRequests",
                newName: "IX_BookingRequests_timeshareId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRequests_Timeshares_timeshareId",
                table: "BookingRequests",
                column: "timeshareId",
                principalTable: "Timeshares",
                principalColumn: "timeshareId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRequests_Timeshares_timeshareId",
                table: "BookingRequests");

            migrationBuilder.RenameColumn(
                name: "timeshareId",
                table: "BookingRequests",
                newName: "roomID");

            migrationBuilder.RenameIndex(
                name: "IX_BookingRequests_timeshareId",
                table: "BookingRequests",
                newName: "IX_BookingRequests_roomID");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Timeshares",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "roomDetailId",
                table: "RoomAmenities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    roomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    timeshareId = table.Column<int>(type: "int", nullable: true),
                    Checkin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Checkout = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nights = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Rooms = table.Column<int>(type: "int", nullable: false),
                    Sleeps = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.roomID);
                    table.ForeignKey(
                        name: "FK_Rooms_Timeshares_timeshareId",
                        column: x => x.timeshareId,
                        principalTable: "Timeshares",
                        principalColumn: "timeshareId");
                });

            migrationBuilder.CreateTable(
                name: "RoomDetail",
                columns: table => new
                {
                    roomDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roomID = table.Column<int>(type: "int", nullable: true),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomDetail", x => x.roomDetailId);
                    table.ForeignKey(
                        name: "FK_RoomDetail_Rooms_roomID",
                        column: x => x.roomID,
                        principalTable: "Rooms",
                        principalColumn: "roomID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timeshares_Id",
                table: "Timeshares",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAmenities_roomDetailId",
                table: "RoomAmenities",
                column: "roomDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomDetail_roomID",
                table: "RoomDetail",
                column: "roomID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_timeshareId",
                table: "Rooms",
                column: "timeshareId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRequests_Rooms_roomID",
                table: "BookingRequests",
                column: "roomID",
                principalTable: "Rooms",
                principalColumn: "roomID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAmenities_RoomDetail_roomDetailId",
                table: "RoomAmenities",
                column: "roomDetailId",
                principalTable: "RoomDetail",
                principalColumn: "roomDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timeshares_AspNetUsers_Id",
                table: "Timeshares",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
