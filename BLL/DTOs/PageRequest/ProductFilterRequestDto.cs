namespace BLL.DTOs.PageRequest
{
    public class ProductFilterRequestDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStock { get; set; }
        public int? CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public string Search { get; set; }
        public double? MinRating { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
