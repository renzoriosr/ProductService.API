using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using ProductService.Domain.Application.Commands;
using ProductService.Domain.Application.Queries;
using ProductService.Domain.Common.Configuration;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Interfaces.Infrastructure;
using ProductService.Domain.Interfaces.Repository;
using ProductService.Domain.Models;

namespace ProductService.Domain.Services
{
    public class ProductService : IRequestHandler<GetProductRequest, Product>,
                                  IRequestHandler<CreateProductCommand, int>,
                                  IRequestHandler<UpdateProductCommand, Unit>
    {
        #region [Fields]
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        private readonly IOptionsSnapshot<CacheConfiguration> _cacheConfiguration;
        private readonly IExternalServiceConnector _externalServiceConnector;
        #endregion

        #region [Constructor]
        public ProductService(IMapper mapper, IProductRepository productRepository,
                              ICacheService cacheService, IOptionsSnapshot<CacheConfiguration> cacheConfiguration,
                              IExternalServiceConnector externalServiceConnector)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _cacheService = cacheService;
            _cacheConfiguration = cacheConfiguration;
            _externalServiceConnector = externalServiceConnector;
        }
        #endregion

        public async Task<Product> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product != null) 
            {
                var allStatusCode = GetAllStatusCodeFromCacheAsync();

                var discount = await GetProductDiscountAsync(request.Id);

                product.Discount = discount;
                product.StatusName = allStatusCode[product.StatusId];
                product.FinalPrice = getFinalPrice(product);
            }
            
            return product;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);

            var productCreatedId = await _productRepository.AddAsync(product);
            return productCreatedId;
        }

        public Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            _productRepository.Update(product);
            return Unit.Task;
        }

        private Dictionary<int, string> GetAllStatusCodeFromCacheAsync()
        {
            var existStatusKey = _cacheService.ExistKey(_cacheConfiguration.Value.StatusCode);

            var statusCode = new Dictionary<int, string>() {
                                { 1, "Active"},
                                { 0, "Inactive"}
                            };

            if (!existStatusKey)
            {
                _cacheService.Set(_cacheConfiguration.Value.StatusCode, statusCode, _cacheConfiguration.Value.CacheTime);
            }

            return _cacheService.Get<Dictionary<int, string>>(_cacheConfiguration.Value.StatusCode);
        }

        private async Task<int?> GetProductDiscountAsync(int productId)
        {
            var apiResponse = await _externalServiceConnector.GetProductDiscountAsync(productId);
            return apiResponse.Discount;
        }

        private decimal getFinalPrice(Product product) 
            => product.Price * (100 - (decimal)product.Discount) / 100;
    }
}
