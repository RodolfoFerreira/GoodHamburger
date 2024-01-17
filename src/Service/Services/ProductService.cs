using Domain.Models;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    internal class ProductService(IProductRepository productRepository) : IProductService
    {

        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IResult> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAll();

                if (products == null) { return Results.NotFound(); }

                return Results.Ok(products);
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

                return Results.Ok(products);
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

                return Results.Ok(products);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
