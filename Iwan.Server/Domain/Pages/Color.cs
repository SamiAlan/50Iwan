namespace Iwan.Server.Domain.Pages
{
    public class Color : BaseEntity
    {
        public string ColorCode { get; set; }
        public string SectionId { get; set; }
        
        public virtual ColorPickingSection Section { get; set; }
    }
}
