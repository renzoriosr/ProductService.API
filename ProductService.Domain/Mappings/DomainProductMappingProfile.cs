using AutoMapper;
using ProductService.Domain.Application.Commands;
using ProductService.Domain.Models;

namespace ProductService.Domain.Mappings
{
    public class DomainProductMappingProfile : Profile
    {
        public DomainProductMappingProfile()
        {
            CreateMap<CreateProductCommand, Product>()
                .ReverseMap();
            CreateMap<UpdateProductCommand, Product>()
                .ReverseMap();
        }
    }
}
