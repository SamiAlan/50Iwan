namespace Iwan.Server.Domain.Pages
{
    public class ContactUsSection : BaseEntity
    {
        public string Location { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
    }
}
