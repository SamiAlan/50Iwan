namespace Iwan.Server.Constants
{
    
    public static class Responses
    {
        /// <summary>
        /// Set of general responses
        /// </summary>
        public static class General
        {
            public const string UnAuthorizedAction = "Unauthorized action";
            public const string ErrorOccured = "An error occured";
            public const string ServiceUnavailable = "Service is unavailable";
            public const string ResourceNotFound = "Resource not found";
            public const string ColorNotFound = "Color not found";
            public const string WorkingOnProducts = "Working on products right now";
        }

        /// <summary>
        /// Set of about-related responses
        /// </summary>
        public static class About
        {
            public const string ImageNotFound = "Image not found";
        }

        /// <summary>
        /// Accounts-specific responses
        /// </summary>
        public static class Accounts
        {
            public const string InvalidLogin = "Invalid login attempt";
            public const string CantDeleteSelf = "Can't delete yourself";
            public const string UserNotFound = "User not found";
            public const string EmailNotVerified = "Account is not verified yet";
            public const string LoginSuccess = "Login successfull";
            public const string InvalidRefresh = "Invalid refresh attempt";
            public const string SignupSuccess = "Registration successful";
            public const string AlreadyExist = "Email is taken";
            public const string ProfileUpdateSuccess = "Profile has been updated successuflly";
            public const string ProfileRetrieved = "Profile has been retrieved successuflly";
            public const string IncorrectPassword = "Incorrect password";
            public const string SystemUserLoginOnly = "Login related to system users only";
            public const string SamePasswords = "Passwords are the same";
            public const string EmailRequired = "Email is required";
            public const string InvalidEmailAddress = "Invalid email address";
            public const string PasswordRequired = "Password is required";
            public const string PasswordMinLength = "Password should be at least 6 characters long";
            public const string SpecialCharactersRequired = "Password should contain at least one special character";
            public const string DigitRequired = "Password should contain at least one digit";
            public const string UpperCharacterRequired = "Password should contain at least one upper-case character";
            public const string ServerErrorWhenDeletingUser = "An error occured at the server while deleting the user";
            public const string ServerErrorWhenAddingUser = "An error occured at the server while adding the user";
            public const string ServerErrorWhenUpdatingUserProfile = "An error occured at the server while updating the user profile";
        }

        /// <summary>
        /// Categories-specific responses
        /// </summary>
        public static class Categories
        {
            public const string ParentCategoryNotExist = "Parent category does not exist";
            public const string ErrorEditingCategoryImage = "An error occured at the server while editing the category image";
            public const string CategorySameNameAlreadyExist = "Category with same name already exists";
            public const string CantExtendSubCategory = "Can't extend a sub-category";
            public const string CategoryNotExist = "Category does not exist";
            public const string AlreadyProductsAttached = "There are already products attached to category '{0}'";
            public const string CategoryHasSubCategories = "Category already has sub-categories under it";
            public const string ServerErrorWhenAddingCategory = "An error occured at the server while adding the category";
            public const string ErrorDeletingCategory = "An error occured at the server while deleting the category";
            public const string CategoryImageNotFound = "Category image could not be found";
            public const string ConflictWithCategories = "A conflict occured with the requestd categories";
            public const string CantExtendSelf = "A category can't extend itself";
        }

        /// <summary>
        /// Settings-specific responses
        /// </summary>
        public static class Settings
        {
            public const string CantWatermarkWithoutImage = "Watermarking requires an image";
        }

        /// <summary>
        /// Settings-specific responses
        /// </summary>
        public static class Jobs
        {
            public const string AlreadyRunningJobOnImages = "A job already working on the images";
        }

        /// <summary>
        /// Images-specific responses
        /// </summary>
        public static class Images
        {
            public const string TempImageNotFound = "Image could not be found";
            public const string ErrorWhileUploading = "An error occured at the server while uploading the image";
            public const string ErrorWhileDeletingImage = "An error occured at the server while deleting the image";
            public const string NotAllImagesExist = "Some images have been deleted already";
            public const string ImageNotFound = "Image not found";
        }
        
        /// <summary>
        /// Sliders-specific responses
        /// </summary>
        public static class Sliders
        {
            public const string ErrorAddingImage = "An error occured at the server while adding the slider image";
            public const string ErrorDeletingSliderImage = "An error occured at the server while deleting the slider image";
            public const string SliderImageNotFound = "Slider image not found";
        }

        /// <summary>
        /// Products-specific responses
        /// </summary>
        public static class Products
        {
            public const string CategoriesNotExist = "Some categories do not exist";
            public const string ProductImageNotFound = "Product image does not exist";
            public const string ErrorDeletingProductImage = "An error occured at the server while deleting the product image";
            public const string ErrorDeletingProduct = "An error occured at the server while deleting the product";
            public const string ProductAlreadyExist = "Product already exists";
            public const string CantAddWithoutANumber = "Can't add a product without a number";
            public const string ErrorAddingProduct = "An error occured at the server while adding the product";
            public const string ImageAlreadyMainImage = "Image is already the main product's image";
            public const string ProductNotFound = "Product does not exist";
            public const string ErrorAddingProductImage = "An error occured at the server while adding the product";
            public const string ProductNotBelongToCategory = "Product '{0}' does not belong to category '{1}'";
            public const string AlreadyBelongToCategory = "Product '{0}' already belongs to category '{1}'";
            public const string ProductCategoryNotFound = "Product category could not be found";
            public const string MainImageNotFound = "Product main image not found";
            public const string NumberAlreadyExists = "Number {0} already exists";
            public const string StateNotFound = "State not found";
            public const string ProductsAlreadySimilar = "Products are already similar";
            public const string ProductsAreNotSimilar = "Products are not similar";
            public const string ErrorChangingProductMainImage = "An error occured at the server while changing the product's main image";
            public const string ProductHasNotMainImage = "Product has no main image";
        }

        /// <summary>
        /// Compositions-specific responses
        /// </summary>
        public static class Compositions
        {
            public const string ErrorAddingComposition = "An error occured at the server while adding the composition";
            public const string ErrorDeletingComposition = "An error occured at the server while deleting the composition";
            public const string ErrorEditingComposition = "An error occured at the server while editing the composition";
            public const string ErrorChangingCompositionImage = "An error occured at the server while changing the composition image";
            public const string CompositionNotExist = "Composition does not exist";
            public const string CompositionSameNameAlreadyExist = "Composition with same name already exists";
        }
        
        /// <summary>
        /// Bills-specific responses
        /// </summary>
        public static class Bills
        {
            public const string BillNotFound = "Bill does not exist";
            public const string CantAddEmptyBill = "Can't add an empty bill";
            public const string NotAllProductsExist = "Some products in the bill do not exist";
            public const string ProductNotEnoughStock = "Not enough quantity of product '{0}'";
            public const string BillItemNotFound = "Bill item not found";
            public const string ErrorAddingBill = "An error occured at the server while adding the bill";
            public const string ErrorAddingBillItem = "An error occured at the server while adding the bill item";
            public const string ErrorEditingBill = "An error occured at the server while editing the bill";
            public const string ErrorEditingBillItem = "An error occured at the server while editing the bill item";
            public const string ErrorRemovingBillItem = "An error occured at the server while removing the bill item";
            public const string ErrorDeletingBill = "An error occured at the server while deleting the bill";
        }

        /// <summary>
        /// Addresses-specific responses
        /// </summary>
        public static class Addresses
        {
            public const string AddressNotFound = "Address not found";
        }

        /// <summary>
        /// Vendors-specific responses
        /// </summary>
        public static class Vendors
        {
            public const string SameNameAlreadyExist = "Vendor with same name already exist";
            public const string VendorNotFound = "Vendor does not exist";
            public const string ErrorAddingVendor = "An error occured at the server while adding the vendor";
            public const string ErrorDeletingVendor = "An error occured at the server while deleting the vendor";
        }
    }
}
