using System;

namespace Iwan.Shared.Options
{
    public class PagedOptions
    {
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => TotalPages > CurrentPage;
    }
}
