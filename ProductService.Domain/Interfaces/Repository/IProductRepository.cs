using ProductService.Domain.Models;

namespace ProductService.Domain.Interfaces.Repository
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Task<Product> GetByIdAsync(int productId);
        Task<int> AddAsync(Product productEntity);
        void Update(Product productEntity);
    }
}
