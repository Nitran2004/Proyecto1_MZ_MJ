using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_MZ_MJ.Migrations
{
    public partial class CambiosBdd3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Habitacion");

            migrationBuilder.AddColumn<double>(
                name: "NumHabitacion",
                table: "Habitacion",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumHabitacion",
                table: "Habitacion");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Habitacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
