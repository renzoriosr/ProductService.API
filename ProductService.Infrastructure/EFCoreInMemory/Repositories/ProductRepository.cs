using AutoMapper;
using ProductService.Domain.Interfaces.Repository;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Configurations;
using ProductService.Infrastructure.EFCoreInMemory.Models;

namespace ProductService.Infrastructure.EFCoreInMemory.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region [Fields]
        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region [Constructor]
        public ProductRepository(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        public List<Product> GetAll()
        {
            var products = _context.Products.ToList<ProductModel>();
            return _mapper.Map<List<Product>>(products);
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            var product = await SearchByProductIdAsync(productId);
            return _mapper.Map<Product>(product);
        }

        public async Task<int> AddAsync(Product productEntity)
        {
            var lastProductId = getLatestProductId(_context);

            productEntity.Id = lastProductId + 1;

            var item = _mapper.Map<ProductModel>(productEntity);

            await _context.Products.AddAsync(item);
            _context.SaveChanges();

            return productEntity.Id;
        }

        public void Update(Product productEntity)
        {
            var item = _mapper.Map<ProductModel>(productEntity);
            _context.Products.Update(item);
            _context.SaveChanges();
        }

        private async Task<ProductModel> SearchByProductIdAsync(int productId)
        {
            var item = await _context.Products.FindAsync(productId);
            return item;
        }

        private int getLatestProductId(ApiContext context) 
        {
            return context.Products.Max(product => product.Id);
        }
    }
}
