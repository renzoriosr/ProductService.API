using MediatR;
using ProductService.Domain.Models;

namespace ProductService.Domain.Application.Queries
{
    public class GetProductRequest : IRequest<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StatusName { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
