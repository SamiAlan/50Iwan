using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iwan.Server.Migrations
{
    public partial class RemoveThumbnailImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutUsSectionImages_Images_CroppedImageId",
                table: "AboutUsSectionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_CroppedImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_ThumbnailImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Images_BackgroundlessCroppedImageId",
                table: "CompositionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Images_BackgroundlessMediumImageId",
                table: "CompositionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Images_BackgroundlessMobileImageId",
                table: "CompositionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Images_BackgroundlessThumbnailImageId",
                table: "CompositionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Images_CroppedImageId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Images_ThumbnailImageId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_CroppedImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_SliderImages_Images_CroppedImageId",
                table: "SliderImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_CroppedImageId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_CompositionImages_BackgroundlessCroppedImageId",
                table: "CompositionImages");

            migrationBuilder.DropIndex(
                name: "IX_CategoryImages_CroppedImageId",
                table: "CategoryImages");

            migrationBuilder.DropColumn(
                name: "ThumbnailImageHeight",
                table: "ProductsImagesSettings");

            migrationBuilder.DropColumn(
                name: "ThumbnailImageWidth",
                table: "ProductsImagesSettings");

            migrationBuilder.DropColumn(
                name: "CroppedImageId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "IsWatermarked",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ThumbnailImageHeight",
                table: "CompositionsImagesSettings");

            migrationBuilder.DropColumn(
                name: "ThumbnailImageWidth",
                table: "CompositionsImagesSettings");

            migrationBuilder.DropColumn(
                name: "BackgroundlessCroppedImageId",
                table: "CompositionImages");

            migrationBuilder.DropColumn(
                name: "CroppedImageId",
                table: "CategoryImages");

            migrationBuilder.DropColumn(
                name: "ThumbnailImageHeight",
                table: "CategoriesImagesSettings");

            migrationBuilder.DropColumn(
                name: "ThumbnailImageWidth",
                table: "CategoriesImagesSettings");

            migrationBuilder.RenameColumn(
                name: "CroppedImageId",
                table: "SliderImages",
                newName: "OriginalImageId");

            migrationBuilder.RenameIndex(
                name: "IX_SliderImages_CroppedImageId",
                table: "SliderImages",
                newName: "IX_SliderImages_OriginalImageId");

            migrationBuilder.RenameColumn(
                name: "CroppedImageId",
                table: "ProductMainImages",
                newName: "OriginalImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_CroppedImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_OriginalImageId");

            migrationBuilder.RenameColumn(
                name: "ThumbnailImageId",
                table: "ProductImages",
                newName: "OriginalImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_ThumbnailImageId",
                table: "ProductImages",
                newName: "IX_ProductImages_OriginalImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessThumbnailImageId",
                table: "CompositionImages",
                newName: "OriginalImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessMobileImageId",
                table: "CompositionImages",
                newName: "MobileImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessMediumImageId",
                table: "CompositionImages",
                newName: "MediumImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CompositionImages_BackgroundlessThumbnailImageId",
                table: "CompositionImages",
                newName: "IX_CompositionImages_OriginalImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CompositionImages_BackgroundlessMobileImageId",
                table: "CompositionImages",
                newName: "IX_CompositionImages_MobileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CompositionImages_BackgroundlessMediumImageId",
                table: "CompositionImages",
                newName: "IX_CompositionImages_MediumImageId");

            migrationBuilder.RenameColumn(
                name: "ThumbnailImageId",
                table: "CategoryImages",
                newName: "OriginalImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_ThumbnailImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_OriginalImageId");

            migrationBuilder.RenameColumn(
                name: "CroppedImageId",
                table: "AboutUsSectionImages",
                newName: "OriginalImageId");

            migrationBuilder.RenameIndex(
                name: "IX_AboutUsSectionImages_CroppedImageId",
                table: "AboutUsSectionImages",
                newName: "IX_AboutUsSectionImages_OriginalImageId");

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailImageId",
                table: "ProductMainImages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutUsSectionImages_Images_OriginalImageId",
                table: "AboutUsSectionImages",
                column: "OriginalImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_OriginalImageId",
                table: "CategoryImages",
                column: "OriginalImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Images_MediumImageId",
                table: "CompositionImages",
                column: "MediumImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Images_MobileImageId",
                table: "CompositionImages",
                column: "MobileImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Images_OriginalImageId",
                table: "CompositionImages",
                column: "OriginalImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Images_OriginalImageId",
                table: "ProductImages",
                column: "OriginalImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_OriginalImageId",
                table: "ProductMainImages",
                column: "OriginalImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SliderImages_Images_OriginalImageId",
                table: "SliderImages",
                column: "OriginalImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutUsSectionImages_Images_OriginalImageId",
                table: "AboutUsSectionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_OriginalImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Images_MediumImageId",
                table: "CompositionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Images_MobileImageId",
                table: "CompositionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CompositionImages_Images_OriginalImageId",
                table: "CompositionImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Images_OriginalImageId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_OriginalImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_SliderImages_Images_OriginalImageId",
                table: "SliderImages");

            migrationBuilder.RenameColumn(
                name: "OriginalImageId",
                table: "SliderImages",
                newName: "CroppedImageId");

            migrationBuilder.RenameIndex(
                name: "IX_SliderImages_OriginalImageId",
                table: "SliderImages",
                newName: "IX_SliderImages_CroppedImageId");

            migrationBuilder.RenameColumn(
                name: "OriginalImageId",
                table: "ProductMainImages",
                newName: "CroppedImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_OriginalImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_CroppedImageId");

            migrationBuilder.RenameColumn(
                name: "OriginalImageId",
                table: "ProductImages",
                newName: "ThumbnailImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_OriginalImageId",
                table: "ProductImages",
                newName: "IX_ProductImages_ThumbnailImageId");

            migrationBuilder.RenameColumn(
                name: "OriginalImageId",
                table: "CompositionImages",
                newName: "BackgroundlessThumbnailImageId");

            migrationBuilder.RenameColumn(
                name: "MobileImageId",
                table: "CompositionImages",
                newName: "BackgroundlessMobileImageId");

            migrationBuilder.RenameColumn(
                name: "MediumImageId",
                table: "CompositionImages",
                newName: "BackgroundlessMediumImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CompositionImages_OriginalImageId",
                table: "CompositionImages",
                newName: "IX_CompositionImages_BackgroundlessThumbnailImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CompositionImages_MobileImageId",
                table: "CompositionImages",
                newName: "IX_CompositionImages_BackgroundlessMobileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CompositionImages_MediumImageId",
                table: "CompositionImages",
                newName: "IX_CompositionImages_BackgroundlessMediumImageId");

            migrationBuilder.RenameColumn(
                name: "OriginalImageId",
                table: "CategoryImages",
                newName: "ThumbnailImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_OriginalImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_ThumbnailImageId");

            migrationBuilder.RenameColumn(
                name: "OriginalImageId",
                table: "AboutUsSectionImages",
                newName: "CroppedImageId");

            migrationBuilder.RenameIndex(
                name: "IX_AboutUsSectionImages_OriginalImageId",
                table: "AboutUsSectionImages",
                newName: "IX_AboutUsSectionImages_CroppedImageId");

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailImageHeight",
                table: "ProductsImagesSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailImageWidth",
                table: "ProductsImagesSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailImageId",
                table: "ProductMainImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CroppedImageId",
                table: "ProductImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsWatermarked",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailImageHeight",
                table: "CompositionsImagesSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailImageWidth",
                table: "CompositionsImagesSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundlessCroppedImageId",
                table: "CompositionImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CroppedImageId",
                table: "CategoryImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailImageHeight",
                table: "CategoriesImagesSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailImageWidth",
                table: "CategoriesImagesSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_CroppedImageId",
                table: "ProductImages",
                column: "CroppedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompositionImages_BackgroundlessCroppedImageId",
                table: "CompositionImages",
                column: "BackgroundlessCroppedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryImages_CroppedImageId",
                table: "CategoryImages",
                column: "CroppedImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutUsSectionImages_Images_CroppedImageId",
                table: "AboutUsSectionImages",
                column: "CroppedImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_CroppedImageId",
                table: "CategoryImages",
                column: "CroppedImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_ThumbnailImageId",
                table: "CategoryImages",
                column: "ThumbnailImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Images_BackgroundlessCroppedImageId",
                table: "CompositionImages",
                column: "BackgroundlessCroppedImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Images_BackgroundlessMediumImageId",
                table: "CompositionImages",
                column: "BackgroundlessMediumImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Images_BackgroundlessMobileImageId",
                table: "CompositionImages",
                column: "BackgroundlessMobileImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompositionImages_Images_BackgroundlessThumbnailImageId",
                table: "CompositionImages",
                column: "BackgroundlessThumbnailImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Images_CroppedImageId",
                table: "ProductImages",
                column: "CroppedImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Images_ThumbnailImageId",
                table: "ProductImages",
                column: "ThumbnailImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_CroppedImageId",
                table: "ProductMainImages",
                column: "CroppedImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SliderImages_Images_CroppedImageId",
                table: "SliderImages",
                column: "CroppedImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
