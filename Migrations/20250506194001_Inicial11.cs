using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto1_MZ_MJ.Migrations
{
    public partial class Inicial11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen",
                table: "Productos",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Productos");
        }
    }
}
