namespace Iwan.Shared.Dtos.Media
{
    public class EditImageDto
    {
        public string Id { get; set; }

        public EditImageDto() { }
        public EditImageDto(string id) => Id = id;
    }
}
