using AutoMapper;
using GoodHamburger.Domain.DTO.Category;
using GoodHamburger.Domain.Models;

namespace GoodHamburger.Infrastructure.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
