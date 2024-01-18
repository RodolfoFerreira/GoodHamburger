using GoodHamburger.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace GoodHamburger.Service.Interfaces
{
    public interface IProductService
    {
        Task<IResult> GetAll();

        Task<IResult> GetAllExtras();

        Task<IResult> GetAllByCategory(EnumCategory productCategory);
    }
}
