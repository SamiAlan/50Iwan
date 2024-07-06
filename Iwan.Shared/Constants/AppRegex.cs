namespace Iwan.Shared.Constants
{
    public static class AppRegex
    {
        // +9639 12345678
        // 09    12345678
        public const string SyrianPhoneNumber = @"^((\+9639(\d){8})|(09(\d){8}))$";
    }
}
