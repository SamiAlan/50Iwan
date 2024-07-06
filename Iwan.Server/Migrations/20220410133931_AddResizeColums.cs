using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iwan.Server.Migrations
{
    public partial class AddResizeColums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateUtc",
                table: "ProductsImagesSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateUtc",
                table: "ProductsImagesSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastResizeDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.UtcNow);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateUtc",
                table: "ProductsImagesSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedDateUtc",
                table: "ProductsImagesSettings");

            migrationBuilder.DropColumn(
                name: "LastResizeDate",
                table: "Products");
        }
    }
}
