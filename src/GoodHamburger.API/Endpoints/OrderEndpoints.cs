using GoodHamburger.Domain.DTO.Order;
using GoodHamburger.Domain.DTO.Product;
using GoodHamburger.Service.Interfaces;

namespace GoodHamburger.API.Endpoints
{
    public static class OrderEndpoints
    {
        public static WebApplication MapOrderEndpoints(this WebApplication app)
        {
            var root = app.MapGroup("/api/orders")
            .WithTags("orders")
            .WithOpenApi();

            root.MapPost("/", async (HttpRequest request, IOrderService orderService, CreateOrderDto order) =>
            {
                return await orderService.Add(order);
            })
            .Produces<OrderDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status304NotModified)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create an order")
            .WithDescription("\n    POST /orders");

            root.MapPut("/", async (HttpRequest request, IOrderService orderService, UpdateOrderDto order) =>
            {
                return await orderService.Update(order);
            })
            .Produces<OrderDto>()
            .ProducesProblem(StatusCodes.Status304NotModified)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update an order")
            .WithDescription("\n    PUT /orders");

            root.MapDelete("/{id}", async (HttpRequest request, IOrderService orderService, int id) =>
            {
                return await orderService.Delete(id);
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status304NotModified)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete an order")
            .WithDescription("\n    DELETE /orders");

            root.MapGet("/", async (HttpRequest request, IOrderService orderService) =>
            {
                return await orderService.GetAll();
            })
            .Produces<List<OrderDto>>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("List all orders")
            .WithDescription("\n    GET /orders");

            return app;
        }
    }
}
