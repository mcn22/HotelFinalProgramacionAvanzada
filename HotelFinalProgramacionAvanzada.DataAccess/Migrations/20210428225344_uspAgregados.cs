using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelFinalProgramacionAvanzada.DataAccess.Migrations
{
    public partial class uspAgregados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC usp_ListarEstadoReserva
                                    AS 
                                    BEGIN 
                                     SELECT * FROM   dbo.EstadosReserva 
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_BuscarEstadoReserva 
                                    @EstadoReservaId int 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM   dbo.EstadosReserva   WHERE  (EstadoReservaId = @EstadoReservaId) 
                                    END ");

            migrationBuilder.Sql(@"CREATE PROC usp_ActualizarEstadoReserva
	                                @EstadoReservaId int,
	                                @NombreEstado varchar(100)
                                    AS 
                                    BEGIN 
                                     UPDATE dbo.EstadosReserva 
                                     SET  NombreEstado = @NombreEstado
                                     WHERE  EstadoReservaId = @EstadoReservaId
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_BorrarEstadoReserva
	                                @EstadoReservaId int
                                    AS 
                                    BEGIN 
                                     DELETE FROM dbo.EstadosReserva 
                                     WHERE  EstadoReservaId = @EstadoReservaId
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_CrearEstadoReserva
                                   @NombreEstado varchar(100)
                                   AS 
                                   BEGIN 
                                    INSERT INTO dbo.EstadosReserva (NombreEstado)
                                    VALUES (@NombreEstado)
                                   END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE usp_ListarEstadoReserva");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_BuscarEstadoReserva");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_ActualizarEstadoReserva");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_BorrarEstadoReserva");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_CrearEstadoReserva");
        }
    }
}
