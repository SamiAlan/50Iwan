using System.Collections.Generic;

namespace Iwan.Server.Domain.Pages
{
    public class ColorPickingSection : BaseEntity
    {
        public virtual ICollection<Color> Colors { get; set; }
    }
}
