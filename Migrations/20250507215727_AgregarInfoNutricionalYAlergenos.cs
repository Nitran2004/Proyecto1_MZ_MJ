using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_MZ_MJ.Migrations
{
    public partial class AgregarInfoNutricionalYAlergenos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alergenos",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InfoNutricional",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alergenos",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "InfoNutricional",
                table: "Productos");
        }
    }
}
