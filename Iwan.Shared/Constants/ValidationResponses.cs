namespace Iwan.Shared.Constants
{
    
    public static class ValidationResponses
    {
        /// <summary>
        /// Set of general validation responses
        /// </summary>
        public static class General
        {
            public const string NameNotEmtpyOrWhitespaceRequired = "Name can't be empty or whitespace";
            public const string ArabicNameNotEmptyOrWhitespace = "Arabic name can't be empty or whitespace";
            public const string EnglishNameNotEmptyOrWhitespace = "English name can't be empty or whitespace";
            public const string InvalidColorCode = "Invalid color code";
            public const string InvalidColorType = "Invalid color type";
            public const string ImageRequired = "Image is required";
            public const string AtLeastOneImageRequired = "At least one image should be uploaded";
            public const string ValueShouldBeGreaterThan = "Value should be greater than {0}";
            public const string ValueShouldBePositive = "Value should be a positive number";
            public const string InvalidPhoneNumber = "Invalid phone number";
            public const string ImageExtensionNotSupported = "Image file extension not supported";
            public const string FieldRequired = "This field is required";
            public const string ValueShouldBeBetweenRangePercent = "Value should be between '{0}' and '{1}' percent";
            public const string MaxLength = "Value too long (max: {0})";
            public const string OnlyRarFilesSupported = "Only rar files supported";
            public const string UnAuthorizedAction = "Unauthorized action";
        }

        /// <summary>
        /// Accounts-specific responses
        /// </summary>
        public static class Accounts
        {
            public const string SamePasswords = "Passwords are the same";
            public const string InvalidRole = "Invalid role";
            public const string PasswordsNotSame = "Password are not equal";
            public const string EmailRequired = "Email is required";
            public const string InvalidEmailAddress = "Invalid email address";
            public const string PasswordRequired = "Password is required";
            public const string PasswordMinLength = "Password should be at least {0} characters long";
            public const string SpecialCharactersRequired = "Password should contain at least one special character";
            public const string DigitRequired = "Password should contain at least one digit";
            public const string UpperCharacterRequired = "Password should contain at least one upper-case character";
        }

        /// <summary>
        /// Categories-specific responses
        /// </summary>
        public static class Categories
        {
        }

        /// <summary>
        /// Settings-specific responses
        /// </summary>
        public static class Settings
        {
        }

        /// <summary>
        /// Images-specific responses
        /// </summary>
        public static class Images
        {
            
        }
        
        /// <summary>
        /// Sliders-specific responses
        /// </summary>
        public static class Sliders
        {
            public const string AnimationTypeError = "Invalid animation type";
        }

        /// <summary>
        /// Products-specific responses
        /// </summary>
        public static class Products
        {
            public const string MainImageNotFound = "Main image not found";
            public const string DetailsFileNotFound = "Details file not found";
            public const string DetailsFileHasNoNumber = "Details file has no number";
            public const string DetailsFileHasNoArabicName = "Details file has no arabic name";
            public const string DetailsFileHasNoEnglishName = "Details file has no english name";
            public const string RarFileRequired = "Rar file is required";
        }

        /// <summary>
        /// Compositions-specific responses
        /// </summary>
        public static class Compositions
        {
            
        }
        
        /// <summary>
        /// Bills-specific responses
        /// </summary>
        public static class Bills
        {
            public const string AtLeastOneBillItem = "Bill should have atleast one item";
        }

        /// <summary>
        /// Vendor-specific responses
        /// </summary>
        public static class Vendors
        {
            public const string VendorDetailsRequired = "Vendor details are required";
        }
    }
}
