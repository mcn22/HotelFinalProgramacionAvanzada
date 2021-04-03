using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelFinalProgramacionAvanzada.DataAccess.Migrations
{
    public partial class nombreHabitacionToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Habitaciones",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Habitaciones");
        }
    }
}
