namespace Iwan.Shared.Dtos.Media
{
    public class AddImageDto
    {
        public string Id { get; set; }

        public AddImageDto() { }
        public AddImageDto(string id) => Id = id;
    }
}
