using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelFinalProgramacionAvanzada.DataAccess.Migrations
{
    public partial class hotelIdOpcionalEmpHotelNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelEmpleados_Hoteles_HotelId",
                table: "HotelEmpleados");

            migrationBuilder.DropIndex(
                name: "IX_HotelEmpleados_HotelId",
                table: "HotelEmpleados");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "HotelEmpleados");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "HotelEmpleados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HotelEmpleados_HotelId",
                table: "HotelEmpleados",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelEmpleados_Hoteles_HotelId",
                table: "HotelEmpleados",
                column: "HotelId",
                principalTable: "Hoteles",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
