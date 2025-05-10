using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_MZ_MJ.Migrations
{
    public partial class AddRelacionesPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_SucursalId",
                table: "Pedidos",
                column: "SucursalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Sucursales_SucursalId",
                table: "Pedidos",
                column: "SucursalId",
                principalTable: "Sucursales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Sucursales_SucursalId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_SucursalId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "Pedidos");
        }
    }
}
