using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_MZ_MJ.Migrations
{
    public partial class AddCollectionPointNombreDescripcion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "CollectionPoints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "CollectionPoints");
        }
    }
}
