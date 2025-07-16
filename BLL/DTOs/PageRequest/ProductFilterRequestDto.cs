namespace BLL.DTOs.PageRequest
{
    public class ProductFilterRequestDto
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 16;
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
        public bool? inStock { get; set; }
        public int? categoryId { get; set; }
        public int? subcategoryId { get; set; }
        public string? search { get; set; }
        public float? minRating { get; set; }
    }
}
