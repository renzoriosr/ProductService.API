using AutoMapper;
using ProductService.Api.V1.Models.Products.Requests;
using ProductService.Api.V1.Models.Products.Responses;
using ProductService.Domain.Application.Commands;
using ProductService.Domain.Models;

namespace ProductService.V1.Mappings
{
    public class ApiProductMappingProfile : Profile
    {
        public ApiProductMappingProfile()
        {
            CreateMap<Product,ProductDetailResponse>()
                .ReverseMap();
            CreateMap<CreateProductRequest, CreateProductCommand>()
                .ReverseMap();
            CreateMap<UpdateProductRequest, UpdateProductCommand>()
                .ReverseMap();
        }
    }
}
