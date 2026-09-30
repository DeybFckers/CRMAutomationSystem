namespace CRMSystem.Models.Responses
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public PaginationMetadata Pagination { get; set; } = new();
    }

    public class PaginationMetadata
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
