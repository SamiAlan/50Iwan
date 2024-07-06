using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos
{
    public class PagedDto<TData>
    {
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("hasNext")]
        public bool HasNext { get; set; }

        [JsonPropertyName("hasPrevious")]
        public bool HasPrevious { get; set; }

        [JsonPropertyName("data")]
        public IList<TData> Data { get; set; }
    }
}
