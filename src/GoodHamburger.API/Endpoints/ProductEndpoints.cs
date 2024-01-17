using Domain.Models;
using Service.Interfaces;

namespace GoodHamburger.API.Endpoints
{
    public static class ProductEndpoints
    {
        public static WebApplication MapProductEndpoints(this WebApplication app)
        {
            var root = app.MapGroup("/api/products")
            .WithTags("products")
            .WithOpenApi();

            root.MapGet("/", async (IProductService productService) =>
            {
                return await productService.GetAll();
            })
            .Produces<List<Product>>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("List all products")
            .WithDescription("\n    GET /products");

            root.MapGet("/sandwiches", async (IProductService productService) => {
                return await productService.GetAllByCategory(EnumCategory.Sandwich);
            })
            .Produces<List<Product>>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(@"List all sandwiches")
            .WithDescription("\n    GET /products/sandwiches");

            root.MapGet("/extras", async (IProductService productService) => {
                return await productService.GetAllExtras();
            })
            .Produces<List<Product>>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary(@"List all extras")
            .WithDescription("\n    GET /products/extras");

            return app;
        }
    }
}
