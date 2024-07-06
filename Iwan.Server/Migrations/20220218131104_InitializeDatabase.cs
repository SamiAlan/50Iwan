using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iwan.Server.Migrations
{
    public partial class InitializeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUsSectionImagesSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MediumImageWidth = table.Column<int>(type: "int", nullable: false),
                    MediumImageHeight = table.Column<int>(type: "int", nullable: false),
                    MobileImageWidth = table.Column<int>(type: "int", nullable: false),
                    MobileImageHeight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsSectionImagesSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsSections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArabicText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CustomerPhone = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsSubCategory = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ParentCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HasSubCategories = table.Column<bool>(type: "bit", nullable: false),
                    HasRelatedProducts = table.Column<bool>(type: "bit", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ColorTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CategoriesImagesSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MediumImageWidth = table.Column<int>(type: "int", nullable: false),
                    MediumImageHeight = table.Column<int>(type: "int", nullable: false),
                    ThumbnailImageWidth = table.Column<int>(type: "int", nullable: false),
                    ThumbnailImageHeight = table.Column<int>(type: "int", nullable: false),
                    MobileImageWidth = table.Column<int>(type: "int", nullable: false),
                    MobileImageHeight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesImagesSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compositions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ArabicDescription = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EnglishDescription = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ColorTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompositionsImagesSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MediumImageWidth = table.Column<int>(type: "int", nullable: false),
                    MediumImageHeight = table.Column<int>(type: "int", nullable: false),
                    ThumbnailImageWidth = table.Column<int>(type: "int", nullable: false),
                    ThumbnailImageHeight = table.Column<int>(type: "int", nullable: false),
                    MobileImageWidth = table.Column<int>(type: "int", nullable: false),
                    MobileImageHeight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompositionsImagesSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUsSections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUsSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeaderSections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArabicTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicSubtitle1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicSubtitle2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishSubtitle1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishSubtitle2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeaderSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    VirtualPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWatermarked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InteriorDesignSections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArabicText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteriorDesignSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobStatusId = table.Column<int>(type: "int", nullable: false),
                    JobTypeId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportRecepientEmail = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsImagesSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MediumImageWidth = table.Column<int>(type: "int", nullable: false),
                    MediumImageHeight = table.Column<int>(type: "int", nullable: false),
                    ThumbnailImageWidth = table.Column<int>(type: "int", nullable: false),
                    ThumbnailImageHeight = table.Column<int>(type: "int", nullable: false),
                    SmallImageWidth = table.Column<int>(type: "int", nullable: false),
                    SmallImageHeight = table.Column<int>(type: "int", nullable: false),
                    MobileImageWidth = table.Column<int>(type: "int", nullable: false),
                    MobileImageHeight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsImagesSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServicesSections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Service1ArabicTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service1ArabicText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service2ArabicTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service2ArabicText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service1EnglishTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service1EnglishText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service2EnglishTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service2EnglishText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SlidersImagesSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MediumImageWidth = table.Column<int>(type: "int", nullable: false),
                    MediumImageHeight = table.Column<int>(type: "int", nullable: false),
                    MobileImageWidth = table.Column<int>(type: "int", nullable: false),
                    MobileImageHeight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlidersImagesSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SmallVersionFileName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    VirtualPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempImagesBackgroundServiceSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TempImagesExpirationDays = table.Column<int>(type: "int", nullable: false),
                    DelayInMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempImagesBackgroundServiceSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ShowOwnProducts = table.Column<bool>(type: "bit", nullable: false),
                    BenefitPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendors_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Jid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false),
                    Invalidated = table.Column<bool>(type: "bit", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsSectionImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AboutUsSectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CroppedImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MediumImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MobileImageId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsSectionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutUsSectionImages_AboutUsSections_AboutUsSectionId",
                        column: x => x.AboutUsSectionId,
                        principalTable: "AboutUsSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AboutUsSectionImages_Images_CroppedImageId",
                        column: x => x.CroppedImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AboutUsSectionImages_Images_MediumImageId",
                        column: x => x.MediumImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AboutUsSectionImages_Images_MobileImageId",
                        column: x => x.MobileImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CategoryImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BackgroundlessCroppedImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessMediumImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessThumbnailImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessMobileImageId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryImages_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CategoryImages_Images_BackgroundlessCroppedImageId",
                        column: x => x.BackgroundlessCroppedImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CategoryImages_Images_BackgroundlessMediumImageId",
                        column: x => x.BackgroundlessMediumImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CategoryImages_Images_BackgroundlessMobileImageId",
                        column: x => x.BackgroundlessMobileImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CategoryImages_Images_BackgroundlessThumbnailImageId",
                        column: x => x.BackgroundlessThumbnailImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompositionImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompositionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessCroppedImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessMediumImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessThumbnailImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessMobileImageId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompositionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompositionImages_Compositions_CompositionId",
                        column: x => x.CompositionId,
                        principalTable: "Compositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompositionImages_Images_BackgroundlessCroppedImageId",
                        column: x => x.BackgroundlessCroppedImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompositionImages_Images_BackgroundlessMediumImageId",
                        column: x => x.BackgroundlessMediumImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompositionImages_Images_BackgroundlessMobileImageId",
                        column: x => x.BackgroundlessMobileImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompositionImages_Images_BackgroundlessThumbnailImageId",
                        column: x => x.BackgroundlessThumbnailImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SliderImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    AnimationTypeId = table.Column<int>(type: "int", nullable: false),
                    CroppedImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MediumImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MobileImageId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SliderImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SliderImages_Images_CroppedImageId",
                        column: x => x.CroppedImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SliderImages_Images_MediumImageId",
                        column: x => x.MediumImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SliderImages_Images_MobileImageId",
                        column: x => x.MobileImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WatermarkSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShouldAddWatermark = table.Column<bool>(type: "bit", nullable: false),
                    Opacity = table.Column<float>(type: "real", nullable: false),
                    WatermarkImageId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatermarkSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatermarkSettings_Images_WatermarkImageId",
                        column: x => x.WatermarkImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InteriorDesignSectionImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InteriorDesignSectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MainImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MobileImageId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteriorDesignSectionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InteriorDesignSectionImages_Images_MainImageId",
                        column: x => x.MainImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InteriorDesignSectionImages_Images_MobileImageId",
                        column: x => x.MobileImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InteriorDesignSectionImages_InteriorDesignSections_InteriorDesignSectionId",
                        column: x => x.InteriorDesignSectionId,
                        principalTable: "InteriorDesignSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ArabicDescription = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DimensionsInArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EnglishDescription = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DimensionsInEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    MakerArabicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MakerEnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ColorTypeId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BillItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BillId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BenefitPercentFromVendor = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillItems_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BillItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CroppedImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MediumImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThumbnailImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SmallImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MobileImageId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Images_CroppedImageId",
                        column: x => x.CroppedImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductImages_Images_MediumImageId",
                        column: x => x.MediumImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductImages_Images_MobileImageId",
                        column: x => x.MobileImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductImages_Images_SmallImageId",
                        column: x => x.SmallImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductImages_Images_ThumbnailImageId",
                        column: x => x.ThumbnailImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductMainImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BackgroundlessCroppedImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessMediumImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessThumbnailImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessSmallImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackgroundlessMobileImageId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMainImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMainImages_Images_BackgroundlessCroppedImageId",
                        column: x => x.BackgroundlessCroppedImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductMainImages_Images_BackgroundlessMediumImageId",
                        column: x => x.BackgroundlessMediumImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductMainImages_Images_BackgroundlessMobileImageId",
                        column: x => x.BackgroundlessMobileImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductMainImages_Images_BackgroundlessSmallImageId",
                        column: x => x.BackgroundlessSmallImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductMainImages_Images_BackgroundlessThumbnailImageId",
                        column: x => x.BackgroundlessThumbnailImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductMainImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductStates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStates_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "44b5b03b-22e6-403a-ad03-04afe58ee61b", "90b3cd61-e045-4535-b0bd-6ad361725460", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9fde253a-f335-4b53-84b8-37e75ac947c1", "026cb531-8460-4a98-a8f1-f9b05c0a7114", "SuperAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "9171c281-6776-42df-9ed8-cd453618d65d", 0, "3d4e078c-abe0-4572-a00f-167d310a884d", "superadmin@aleman-museum.com", true, false, null, "Admin", "SUPERADMIN@ALEMAN-MUSEUM.COM", "SUPERADMIN@ALEMAN-MUSEUM.COM", "AQAAAAEAACcQAAAAEGSb01b6/VHby5nbm141lKEaO30VocTMDvEa1vfzqUDPEAKc1OzWfTfp59LdooGZxg==", null, false, "7c2da1b5-12e4-4463-a3e2-06c3d0461129", false, "admin5245" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9fde253a-f335-4b53-84b8-37e75ac947c1", "9171c281-6776-42df-9ed8-cd453618d65d" });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsSectionImages_AboutUsSectionId",
                table: "AboutUsSectionImages",
                column: "AboutUsSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsSectionImages_CroppedImageId",
                table: "AboutUsSectionImages",
                column: "CroppedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsSectionImages_MediumImageId",
                table: "AboutUsSectionImages",
                column: "MediumImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsSectionImages_MobileImageId",
                table: "AboutUsSectionImages",
                column: "MobileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BillItems_BillId",
                table: "BillItems",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillItems_ProductId",
                table: "BillItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryImages_BackgroundlessCroppedImageId",
                table: "CategoryImages",
                column: "BackgroundlessCroppedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryImages_BackgroundlessMediumImageId",
                table: "CategoryImages",
                column: "BackgroundlessMediumImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryImages_BackgroundlessMobileImageId",
                table: "CategoryImages",
                column: "BackgroundlessMobileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryImages_BackgroundlessThumbnailImageId",
                table: "CategoryImages",
                column: "BackgroundlessThumbnailImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryImages_CategoryId",
                table: "CategoryImages",
                column: "CategoryId",
                unique: true,
                filter: "[CategoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompositionImages_BackgroundlessCroppedImageId",
                table: "CompositionImages",
                column: "BackgroundlessCroppedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompositionImages_BackgroundlessMediumImageId",
                table: "CompositionImages",
                column: "BackgroundlessMediumImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompositionImages_BackgroundlessMobileImageId",
                table: "CompositionImages",
                column: "BackgroundlessMobileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompositionImages_BackgroundlessThumbnailImageId",
                table: "CompositionImages",
                column: "BackgroundlessThumbnailImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompositionImages_CompositionId",
                table: "CompositionImages",
                column: "CompositionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InteriorDesignSectionImages_InteriorDesignSectionId",
                table: "InteriorDesignSectionImages",
                column: "InteriorDesignSectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InteriorDesignSectionImages_MainImageId",
                table: "InteriorDesignSectionImages",
                column: "MainImageId");

            migrationBuilder.CreateIndex(
                name: "IX_InteriorDesignSectionImages_MobileImageId",
                table: "InteriorDesignSectionImages",
                column: "MobileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductId",
                table: "ProductCategories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_CroppedImageId",
                table: "ProductImages",
                column: "CroppedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_MediumImageId",
                table: "ProductImages",
                column: "MediumImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_MobileImageId",
                table: "ProductImages",
                column: "MobileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_SmallImageId",
                table: "ProductImages",
                column: "SmallImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ThumbnailImageId",
                table: "ProductImages",
                column: "ThumbnailImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainImages_BackgroundlessCroppedImageId",
                table: "ProductMainImages",
                column: "BackgroundlessCroppedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainImages_BackgroundlessMediumImageId",
                table: "ProductMainImages",
                column: "BackgroundlessMediumImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainImages_BackgroundlessMobileImageId",
                table: "ProductMainImages",
                column: "BackgroundlessMobileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainImages_BackgroundlessSmallImageId",
                table: "ProductMainImages",
                column: "BackgroundlessSmallImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainImages_BackgroundlessThumbnailImageId",
                table: "ProductMainImages",
                column: "BackgroundlessThumbnailImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainImages_ProductId",
                table: "ProductMainImages",
                column: "ProductId",
                unique: true,
                filter: "[ProductId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorId",
                table: "Products",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStates_ProductId",
                table: "ProductStates",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SliderImages_CroppedImageId",
                table: "SliderImages",
                column: "CroppedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_SliderImages_MediumImageId",
                table: "SliderImages",
                column: "MediumImageId");

            migrationBuilder.CreateIndex(
                name: "IX_SliderImages_MobileImageId",
                table: "SliderImages",
                column: "MobileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_AddressId",
                table: "Vendors",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_WatermarkSettings_WatermarkImageId",
                table: "WatermarkSettings",
                column: "WatermarkImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUsSectionImages");

            migrationBuilder.DropTable(
                name: "AboutUsSectionImagesSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BillItems");

            migrationBuilder.DropTable(
                name: "CategoriesImagesSettings");

            migrationBuilder.DropTable(
                name: "CategoryImages");

            migrationBuilder.DropTable(
                name: "CompositionImages");

            migrationBuilder.DropTable(
                name: "CompositionsImagesSettings");

            migrationBuilder.DropTable(
                name: "ContactUsSections");

            migrationBuilder.DropTable(
                name: "HeaderSections");

            migrationBuilder.DropTable(
                name: "InteriorDesignSectionImages");

            migrationBuilder.DropTable(
                name: "JobDetails");

            migrationBuilder.DropTable(
                name: "MailSettings");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductMainImages");

            migrationBuilder.DropTable(
                name: "ProductsImagesSettings");

            migrationBuilder.DropTable(
                name: "ProductStates");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "ServicesSections");

            migrationBuilder.DropTable(
                name: "SliderImages");

            migrationBuilder.DropTable(
                name: "SlidersImagesSettings");

            migrationBuilder.DropTable(
                name: "TempImages");

            migrationBuilder.DropTable(
                name: "TempImagesBackgroundServiceSettings");

            migrationBuilder.DropTable(
                name: "WatermarkSettings");

            migrationBuilder.DropTable(
                name: "AboutUsSections");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Compositions");

            migrationBuilder.DropTable(
                name: "InteriorDesignSections");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
