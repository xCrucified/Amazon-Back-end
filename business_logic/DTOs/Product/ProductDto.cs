namespace business_logic.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int InStock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public List<ReviewDto> Reviews { get; set; }
        public List<ProductImageDto> Images { get; set; }
    }
}
