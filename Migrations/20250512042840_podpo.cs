using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_MZ_MJ.Migrations
{
    public partial class podpo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PuntoRecoleccionId",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_PuntoRecoleccionId",
                table: "Pedidos",
                column: "PuntoRecoleccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_CollectionPoints_PuntoRecoleccionId",
                table: "Pedidos",
                column: "PuntoRecoleccionId",
                principalTable: "CollectionPoints",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_CollectionPoints_PuntoRecoleccionId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_PuntoRecoleccionId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "PuntoRecoleccionId",
                table: "Pedidos");
        }
    }
}
