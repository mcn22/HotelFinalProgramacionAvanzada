using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelFinalProgramacionAvanzada.DataAccess.Migrations
{
    public partial class hotelIdOpcionalUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HotelId",
                table: "AspNetUsers",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Hoteles_HotelId",
                table: "AspNetUsers",
                column: "HotelId",
                principalTable: "Hoteles",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Hoteles_HotelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HotelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "AspNetUsers");
        }
    }
}
