using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_MZ_MJ.Migrations
{
    public partial class AddPuntosRecoleccion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "CollectionPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionPoints_SucursalId",
                table: "CollectionPoints",
                column: "SucursalId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionPoints_Sucursales_SucursalId",
                table: "CollectionPoints",
                column: "SucursalId",
                principalTable: "Sucursales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionPoints_Sucursales_SucursalId",
                table: "CollectionPoints");

            migrationBuilder.DropIndex(
                name: "IX_CollectionPoints_SucursalId",
                table: "CollectionPoints");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "CollectionPoints");
        }
    }
}
