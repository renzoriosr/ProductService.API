using ProductService.Api.V1.Validators;

namespace ProductService.Api.V1.Models.Products.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public int StatusId { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
