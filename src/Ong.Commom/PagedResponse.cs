namespace Ong.Commom
{
    public class PagedResponse<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int PageSize { get; set; }

        public PagedResponse(
            IReadOnlyList<T> items,
            int total,
            int skip,
            int pageSize)
        {
            Items = items;
            Total = total;
            Skip = skip;
            PageSize = pageSize;
        }
    }
}
