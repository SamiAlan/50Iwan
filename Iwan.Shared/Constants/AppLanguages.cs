namespace Iwan.Shared.Constants
{
    public static class AppLanguages
    {
        public static class English
        {
            public const string Culture = "en-US";
            public const string Text = "English";
        }

        public static class Arabic
        {
            public const string Culture = "ar-SA";
            public const string Text = "العربية";
        }

        public static string[] All()
            => new string[] { English.Culture, Arabic.Culture };
    }
}
