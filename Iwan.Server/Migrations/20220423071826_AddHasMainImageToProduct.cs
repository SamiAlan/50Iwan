using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iwan.Server.Migrations
{
    public partial class AddHasMainImageToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasMainImage",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasMainImage",
                table: "Products");
        }
    }
}
