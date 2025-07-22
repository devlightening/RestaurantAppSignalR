using SignalR.DtoLayer.CategoryDto;

namespace SignalR.DtoLayer.ProductDto
{
    public class ResultProductWithCategory
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool ProductStatus { get; set; }
        public ResultCategoryDto Category { get; set; }
    }
}
