using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iwan.Server.Migrations
{
    public partial class RemoveAnimationTypeFromSliderImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimationTypeId",
                table: "SliderImages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimationTypeId",
                table: "SliderImages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
