namespace Iwan.Server.Domain.Common
{
    /// <summary>
    /// Type of color which defines how to get the color for an entity
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// Means that the color should be taken from the entity itself
        /// </summary>
        Custom = 1,

        /// <summary>
        /// Meana that the color should be taken from the parent entity
        /// </summary>
        FromParent = 2,

        /// <summary>
        /// Means that no color should be used
        /// </summary>
        NoChange = 3,
    }
}
