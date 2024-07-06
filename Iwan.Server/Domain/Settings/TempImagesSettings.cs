namespace Iwan.Server.Domain.Settings
{
    public class TempImagesSettings : BaseEntity
    {
        // Temp images expiration
        public int TempImagesExpirationDays { get; set; } = 7;

        public int DelayInMinutes { get; set; }
    }
}
