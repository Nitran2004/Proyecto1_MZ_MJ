using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_MZ_MJ.Migrations
{
    public partial class PedidoProducto_Total : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "PedidoProductos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "PedidoProductos");
        }
    }
}
