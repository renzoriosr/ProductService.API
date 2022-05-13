namespace ProductService.Api.V1.Models.Products.Requests
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }
        public int StatusId { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
