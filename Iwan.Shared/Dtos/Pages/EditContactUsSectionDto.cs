using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class EditContactUsSectionDto
    {
        public string Location { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
    }
}
