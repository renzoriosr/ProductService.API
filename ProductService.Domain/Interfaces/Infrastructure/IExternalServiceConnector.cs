using ProductService.Domain.Models;

namespace ProductService.Domain.Interfaces.Infrastructure
{
    public interface IExternalServiceConnector
    {
        Task<Product> GetProductDiscountAsync(int productId);
    }
}
