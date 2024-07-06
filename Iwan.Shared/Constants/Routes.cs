namespace Iwan.Shared.Constants
{
    public static class Routes
    {
        public static class Api
        {
            public static class Admin
            {
                public const string BASE = "/api/admin";

                public static class Dashboard
                {
                    public const string BASE = "/api/admin/dashboard";
                }

                public static class Downloads
                {
                    public const string ImageManager = "/downloads/image-manager";
                }

                public static class Pages
                {
                    public const string BASE = "/api/admin/pages";
                    public const string BASE_HOME = BASE + "/home";
                    public const string BASE_PRODUCT_DETAILS = BASE + "/product-details";
                    public const string BASE_CONTACTUS = BASE_HOME + "/contact-us";
                    public const string BASE_SERVICES = BASE_HOME + "/services";
                    public const string BASE_HEADER = BASE_HOME + "/header";
                    public const string BASE_ABOUTUS = BASE_HOME + "/about-us";
                    public const string BASE_INTERIOR_DESIGN = BASE_HOME + "/interior-design";
                    public const string BASE_ABOUTUS_IMAGES = BASE_PRODUCT_DETAILS + "/colors";
                    public const string BASE_COLORS = BASE_ABOUTUS + "/images";
                    public const string ChangeInteriorDesignMainImage = BASE_INTERIOR_DESIGN + "/change-main-image";
                    public const string ChangeInteriorDesignMobileImage = BASE_INTERIOR_DESIGN + "/change-mobile-image";
                    public const string DeleteAboutUsImage = BASE_ABOUTUS_IMAGES + "/{id}";
                    public const string DeleteColor = BASE_COLORS + "/{id}";
                }

                public static class Accounts
                {
                    public const string BASE = "/api/admin/accounts";

                    public const string Profile = BASE + "/profile";
                    public const string Login = BASE + "/login";
                    public const string AddAdminUser = BASE + "/add-admin-user";
                    public const string RefreshToken = BASE + "/refresh-token";
                    public const string ChangePassword = BASE + "/change-password";
                    public const string Delete = BASE + "/{id}";
                }

                public static class Categories
                {
                    public const string BASE = "/api/admin/categories";

                    public const string GetAll = BASE + "/getall";
                    public const string GetCategory = BASE + "/{id}";
                    public const string Delete = BASE + "/{id}";
                    public const string FlipVisibility = BASE + "/{id}/flip-visibility";
                    public const string EditImage = BASE + "/edit-image";
                    public const string ChangeImage = BASE + "/change-image";
                }

                public static class Jobs
                {
                    public const string BASE = "/api/admin/jobs";

                    public const string StartWatermarking = "/api/admin/jobs/start-watermarking";
                    public const string StartUnWatermarking = "/api/admin/jobs/start-unwatermarking";
                    public const string StartProductsResizing = "/api/admin/jobs/start-products-resizing";
                    public const string StartCategoriesResizing = "/api/admin/jobs/start-categories-resizing";
                    public const string StartCompositionsResizing = "/api/admin/jobs/start-compositions-resizing";
                    public const string StartSliderImagesResizing = "/api/admin/jobs/start-sliderimages-resizing";
                    public const string StartAboutUsResizing = "/api/admin/jobs/start-aboutus-resizing";
                }

                public static class Bills
                {
                    public const string BASE = "/api/admin/bills";
                    public const string BASE_BILL_ITEM = BASE + "/billitems";

                    public const string GetBill = BASE + "/{id}";
                    public const string Delete = BASE + "/{id}";
                    public const string DeleteBillItem = BASE_BILL_ITEM + "/{id}";
                }

                public static class Compositions
                {
                    public const string BASE = "/api/admin/compositions";

                    public const string GetComposition = BASE + "/{id}";
                    public const string Delete = BASE + "/{id}";
                    public const string EditImage = BASE + "/edit-image";
                }

                public static class Activities
                {
                    public const string BASE = "/api/admin/activities";
                }

                public static class Settings
                {
                    public const string BASE = "/api/admin/settings";

                    public const string Images = BASE + "/images";
                    public const string Images_Products = Images + "/products";
                    public const string Images_Categories = Images + "/categories";
                    public const string Images_AboutUsSection = Images + "/about-us";
                    public const string Images_InteriorDesignSection = Images + "/interior-design";
                    public const string Images_Compositions = Images + "/compositions";
                    public const string Images_Slider_Images = Images + "/slider-images";
                    public const string TEMPIMAGES_BASE = BASE + "/temp-images";
                    public const string WATERMARK_BASE = BASE + "/watermark";
                    public const string ChangeWatermarkImage = WATERMARK_BASE + "/change-image";
                }

                public static class Sliders
                {
                    public const string BASE = "/api/admin/sliders";
                    public const string GetSlider = BASE + "/{id}";
                    public const string Delete = BASE + "/{id}";
                }

                public static class Products
                {
                    public const string BASE = "/api/admin/products";
                    public const string BASE_CATEGORIES = BASE + "/categories";
                    public const string BASE_IMAGES = BASE + "/images";
                    public const string BASE_STATES = BASE + "/states";
                    public const string BASE_SIMILAR = BASE + "/similar-products";

                    public const string GetAll = BASE + "/get-all";
                    public const string GetProduct = BASE + "/{id}";
                    public const string GetImages = BASE + "/{id}/images";
                    public const string GetStates = BASE + "/{id}/states";
                    public const string GetSimilarProducts = BASE + "/{id}/similar-products";
                    public const string GetProductCategories = BASE + "/{id}/categories";
                    public const string AddViaRarFile = BASE + "/add-via-rar";
                    public const string ChangeMainImage = BASE + "/change-main-image";
                    public const string DeleteProduct = BASE + "/{id}";
                    public const string DeleteProductCategory = BASE_CATEGORIES + "/{id}";
                    public const string DeleteProductImage = BASE_IMAGES + "/{id}";
                    public const string DeleteState = BASE_STATES + "/{id}";
                    public const string DeleteSimilarProduct = BASE_SIMILAR + "/{id}";

                    public const string ResizeImage = BASE + "/{id}" + "/images/resize";
                    public const string WatermarkImage = BASE + "/{id}" + "/images/watermark";
                    public const string UnWatermarkImage = BASE + "/{id}" + "/images/unwatermark";
                    public const string FlipVisibility = BASE + "/{id}" + "/flip-visibility";
                }

                public static class Images
                {
                    public const string BASE = "/api/admin/images";
                    public const string BASE_TEMP = "/api/admin/images/temp";

                    public const string Delete = BASE + "/{id}";
                    public const string DeleteTemp = BASE_TEMP + "/{id}";
                }

                public static class Vendors
                {
                    public const string BASE = "/api/admin/vendors";

                    public const string GetVendor = BASE + "/{id}";
                    public const string GetAll = BASE + "/getall";
                    public const string Delete = BASE + "/{id}";
                }

                public static class Addresses
                {
                    public const string BASE = "/api/admin/addresses";

                    public const string GetAddress = BASE + "/{id}";
                    public const string Delete = BASE + "/{id}";
                }
            }
        }

        public static class Accounts
        {
            public const string Login = "/account/login";
            public const string Register = "/account/register";
        }

    }
}
