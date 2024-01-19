using GoodHamburger.Domain.Models;
using Microsoft.AspNetCore.Http;
using GoodHamburger.Repository.Interfaces;
using GoodHamburger.Service.Interfaces;
using AutoMapper;
using GoodHamburger.Domain.DTO.Product;

namespace GoodHamburger.Service.Services
{
    public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
    {

        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAll();

                if (products == null) { return Results.NotFound(); }

                return Results.Ok(_mapper.Map<List<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> GetAllExtras()
        {
            try
            {
                var products = await _productRepository.GetAllExtras();

                if (products == null) { return Results.NotFound(); }

                return Results.Ok(_mapper.Map<List<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> GetAllByCategory(EnumCategory productCategory)
        {
            try
            {
                var products = await _productRepository.GetAllByCategory(productCategory);

                if (products == null) { return Results.NotFound(); }

                return Results.Ok(_mapper.Map<List<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
