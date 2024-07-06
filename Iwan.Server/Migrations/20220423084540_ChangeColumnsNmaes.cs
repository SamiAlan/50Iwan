using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iwan.Server.Migrations
{
    public partial class ChangeColumnsNmaes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_BackgroundlessCroppedImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_BackgroundlessMediumImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_BackgroundlessMobileImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_BackgroundlessThumbnailImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessCroppedImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessMediumImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessMobileImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessSmallImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessThumbnailImageId",
                table: "ProductMainImages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "445afcbd-5ac1-4420-a1c7-b9c7e5cd23cd");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "98f4853c-d705-4300-9120-d46f4f28df2a", "841ef4df-ebe4-46e1-9645-81b8bf0ce782" });

            migrationBuilder.RenameColumn(
                name: "BackgroundlessThumbnailImageId",
                table: "ProductMainImages",
                newName: "ThumbnailImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessSmallImageId",
                table: "ProductMainImages",
                newName: "SmallImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessMobileImageId",
                table: "ProductMainImages",
                newName: "MobileImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessMediumImageId",
                table: "ProductMainImages",
                newName: "MediumImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessCroppedImageId",
                table: "ProductMainImages",
                newName: "CroppedImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_BackgroundlessThumbnailImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_ThumbnailImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_BackgroundlessSmallImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_SmallImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_BackgroundlessMobileImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_MobileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_BackgroundlessMediumImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_MediumImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_BackgroundlessCroppedImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_CroppedImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessThumbnailImageId",
                table: "CategoryImages",
                newName: "ThumbnailImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessMobileImageId",
                table: "CategoryImages",
                newName: "MobileImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessMediumImageId",
                table: "CategoryImages",
                newName: "MediumImageId");

            migrationBuilder.RenameColumn(
                name: "BackgroundlessCroppedImageId",
                table: "CategoryImages",
                newName: "CroppedImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_BackgroundlessThumbnailImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_ThumbnailImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_BackgroundlessMobileImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_MobileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_BackgroundlessMediumImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_MediumImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_BackgroundlessCroppedImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_CroppedImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_CroppedImageId",
                table: "CategoryImages",
                column: "CroppedImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_MediumImageId",
                table: "CategoryImages",
                column: "MediumImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_MobileImageId",
                table: "CategoryImages",
                column: "MobileImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_ThumbnailImageId",
                table: "CategoryImages",
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
                name: "FK_ProductMainImages_Images_MediumImageId",
                table: "ProductMainImages",
                column: "MediumImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_MobileImageId",
                table: "ProductMainImages",
                column: "MobileImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_SmallImageId",
                table: "ProductMainImages",
                column: "SmallImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_ThumbnailImageId",
                table: "ProductMainImages",
                column: "ThumbnailImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_CroppedImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_MediumImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_MobileImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Images_ThumbnailImageId",
                table: "CategoryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_CroppedImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_MediumImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_MobileImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_SmallImageId",
                table: "ProductMainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMainImages_Images_ThumbnailImageId",
                table: "ProductMainImages");

            migrationBuilder.RenameColumn(
                name: "ThumbnailImageId",
                table: "ProductMainImages",
                newName: "BackgroundlessThumbnailImageId");

            migrationBuilder.RenameColumn(
                name: "SmallImageId",
                table: "ProductMainImages",
                newName: "BackgroundlessSmallImageId");

            migrationBuilder.RenameColumn(
                name: "MobileImageId",
                table: "ProductMainImages",
                newName: "BackgroundlessMobileImageId");

            migrationBuilder.RenameColumn(
                name: "MediumImageId",
                table: "ProductMainImages",
                newName: "BackgroundlessMediumImageId");

            migrationBuilder.RenameColumn(
                name: "CroppedImageId",
                table: "ProductMainImages",
                newName: "BackgroundlessCroppedImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_ThumbnailImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_BackgroundlessThumbnailImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_SmallImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_BackgroundlessSmallImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_MobileImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_BackgroundlessMobileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_MediumImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_BackgroundlessMediumImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMainImages_CroppedImageId",
                table: "ProductMainImages",
                newName: "IX_ProductMainImages_BackgroundlessCroppedImageId");

            migrationBuilder.RenameColumn(
                name: "ThumbnailImageId",
                table: "CategoryImages",
                newName: "BackgroundlessThumbnailImageId");

            migrationBuilder.RenameColumn(
                name: "MobileImageId",
                table: "CategoryImages",
                newName: "BackgroundlessMobileImageId");

            migrationBuilder.RenameColumn(
                name: "MediumImageId",
                table: "CategoryImages",
                newName: "BackgroundlessMediumImageId");

            migrationBuilder.RenameColumn(
                name: "CroppedImageId",
                table: "CategoryImages",
                newName: "BackgroundlessCroppedImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_ThumbnailImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_BackgroundlessThumbnailImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_MobileImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_BackgroundlessMobileImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_MediumImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_BackgroundlessMediumImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryImages_CroppedImageId",
                table: "CategoryImages",
                newName: "IX_CategoryImages_BackgroundlessCroppedImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_BackgroundlessCroppedImageId",
                table: "CategoryImages",
                column: "BackgroundlessCroppedImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_BackgroundlessMediumImageId",
                table: "CategoryImages",
                column: "BackgroundlessMediumImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_BackgroundlessMobileImageId",
                table: "CategoryImages",
                column: "BackgroundlessMobileImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Images_BackgroundlessThumbnailImageId",
                table: "CategoryImages",
                column: "BackgroundlessThumbnailImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessCroppedImageId",
                table: "ProductMainImages",
                column: "BackgroundlessCroppedImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessMediumImageId",
                table: "ProductMainImages",
                column: "BackgroundlessMediumImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessMobileImageId",
                table: "ProductMainImages",
                column: "BackgroundlessMobileImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessSmallImageId",
                table: "ProductMainImages",
                column: "BackgroundlessSmallImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMainImages_Images_BackgroundlessThumbnailImageId",
                table: "ProductMainImages",
                column: "BackgroundlessThumbnailImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
