using AutoMapper;
using GoodHamburger.Domain.DTO.Product;
using GoodHamburger.Domain.Models;

namespace GoodHamburger.Infrastructure.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
