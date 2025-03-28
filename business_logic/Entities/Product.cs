using Newtonsoft.Json;

namespace business_logic.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public int SubcategoryId { get; set; }
        [JsonIgnore]
        public Subcategory Subcategory { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<ProductImage>? ProductImages { get; set; }
        public ICollection<ProductProperties>? ProductProperties { get; set; }
    }
}
