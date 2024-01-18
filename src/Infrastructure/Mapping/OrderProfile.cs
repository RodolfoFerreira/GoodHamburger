using AutoMapper;
using GoodHamburger.Domain.DTO.Order;
using GoodHamburger.Domain.Models;

namespace GoodHamburger.Infrastructure.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
