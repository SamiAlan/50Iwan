using Iwan.Server.Domain.Common;

namespace Iwan.Server.Domain.Compositions
{
    public class Composition : BaseEntity
    {
        /// <summary>
        /// Gets or sets the arabic name
        /// </summary>
        public string ArabicName { get; set; }

        /// <summary>
        /// Gets or sets the english name
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// Gets or sets the arabic description
        /// </summary>
        public string ArabicDescription { get; set; }

        /// <summary>
        /// Gets or sets the english description
        /// </summary>
        public string EnglishDescription { get; set; }

        /// <summary>
        /// Gets or sets the color code
        /// </summary>
        public string ColorCode { get; set; }

        /// <summary>
        /// Gets or sets an indicator for whether the composition is visible or not
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the color type identifier
        /// </summary>
        public int ColorTypeId { get; set; }

        /// <summary>
        /// Gets or sets the color type
        /// </summary>
        public ColorType ColorType
        {
            get => (ColorType)ColorTypeId;
            set => ColorTypeId = (int)value;
        }

        public virtual CompositionImage Image { get; set; }
    }
}
