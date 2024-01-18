using GoodHamburger.Domain.DTO.Order;
using GoodHamburger.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace GoodHamburger.Service.Interfaces
{
    public interface IOrderService
    {
        Task<IResult> Add(CreateOrderDto order);
        Task<IResult> Update(UpdateOrderDto order);
        Task<IResult> Delete(int id);
        Task<IResult> GetAll();
    }
}
