using AutoMapper;
using GoodHamburger.Domain.DTO.OrderProduct;
using GoodHamburger.Domain.Models;

namespace GoodHamburger.Infrastructure.Mapping
{
    public class OrderProductProfile : Profile
    {
        public OrderProductProfile()
        {
            CreateMap<OrderProduct, CreateOrderProductDto>().ReverseMap();
            CreateMap<OrderProduct, OrderProductDto>().ReverseMap();
        }
    }
}
