using AutoMapper;
using ProductService.Domain.Models;
using ProductService.Infrastructure.EFCoreInMemory.Models;

namespace ProductService.Infrastructure.Mappings
{
    public class EFCoreMappingProfile : Profile
    {
        public EFCoreMappingProfile()
        {
            CreateMap<ProductModel, Product>()
                .ReverseMap();
        }
    }
}
