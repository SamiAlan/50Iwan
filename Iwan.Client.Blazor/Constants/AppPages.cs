namespace Iwan.Client.Blazor.Constants
{
    public static class AppPages
    {
        public const string Login = "/admin/login";

        public const string Index = "/admin";

        public const string Products = "/admin/products";
        public const string AddProduct = "/admin/products/add";
        public const string EditProduct = "/admin/products/{Id}/edit";
        public const string ProductCategories = "/admin/products/{Id}/categories";
        public const string ProductImages = "/admin/products/{Id}/images";

        public const string Categories = "/admin/categories";
        public const string AddCategory = "/admin/categories/add";
        public const string EditCategory = "/admin/categories/{Id}/edit";

        public const string Vendors = "/admin/vendors";
        public const string AddVendor = "/admin/vendors/add";
        public const string EditVendor = "/admin/vendors/{Id}/edit";

        public const string Compositions = "/admin/compositions";
        public const string AddComposition = "/admin/compositions/add";
        public const string EditComposition = "/admin/compositions/{Id}/edit";

        public const string SliderImages = "/admin/sliderimages";

        public const string ImagesSettings = "/admin/settings/images";
        public const string TempImagesSettings = "/admin/settings/tempimages";
        public const string WatermarkSettings = "/admin/settings/watermark";

        public const string Users = "/admin/users";
        public const string AddUser = "/admin/users/add";
        public const string Account = "/admin/account";

        public const string Bills = "/admin/bills";
        public const string AddBill = "/admin/bills/add";
        public const string EditBills = "/admin/bills/{Id}/edit";

        public const string HomePage = "/admin/pages/home";
        public const string ProductDetailsPage = "/admin/pages/product-details";

        public const string BackgroundJobs = "/admin/jobs/background";
    }
}
