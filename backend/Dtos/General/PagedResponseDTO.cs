namespace Jannara_Ecommerce.DTOs.General
{
    public class PagedResponseDTO<T> where T : class
    {
        public PagedResponseDTO(int total, int pageNumber, int pageSize, IEnumerable<T> items)
        {
            Total = total;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Items = items;
        }

        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
