using AutoMapper;
using FluentValidation;
using GoodHamburger.Domain.DTO.Order;
using GoodHamburger.Domain.Models;
using GoodHamburger.Repository.Interfaces;
using GoodHamburger.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.ObjectModel;

namespace GoodHamburger.Service.Services
{
    public class OrderService(IOrderRepository orderRepository,
        IProductRepository productRepository,
        IOrderProductRepository orderProductRepository,
        IDiscountService discountService,
        IValidator<CreateOrderDto> createOrderDtoValidator,
        IValidator<UpdateOrderDto> updateOrderDtoValidator,
        IValidator<Order> orderValidator,
        IMapper mapper) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IOrderProductRepository _orderProductRepository = orderProductRepository;
        private readonly IValidator<CreateOrderDto> _createOrderDtoValidator = createOrderDtoValidator;
        private readonly IValidator<UpdateOrderDto> _updateOrderDtoValidator = updateOrderDtoValidator;
        private readonly IValidator<Order> _orderValidator = orderValidator;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult> Add(CreateOrderDto order)
        {
            try
            {
                var validateRequestResult = await _createOrderDtoValidator.ValidateAsync(order);
                if (!validateRequestResult.IsValid) { return Results.ValidationProblem(validateRequestResult.ToDictionary()); }

                var orderObject = _mapper.Map<Order>(order);

                var products = await _productRepository.GetAllByIds(orderObject.OrderProducts.Select(p => p.ProductId));

                orderObject.OrderProducts = new Collection<OrderProduct>(
                                                (
                                                    from orderProduct in orderObject.OrderProducts
                                                    join product in products on orderProduct.ProductId equals product.Id
                                                    select new OrderProduct
                                                    {
                                                        Product = product,
                                                        Quantity = orderProduct.Quantity,
                                                        ProductId = orderProduct.ProductId
                                                    }).ToList()
                                                );

                var validateOrderResult = await _orderValidator.ValidateAsync(orderObject);
                if (!validateOrderResult.IsValid) { return Results.ValidationProblem(validateOrderResult.ToDictionary()); }

                orderObject.GrossValue = orderObject.OrderProducts.Sum(x => x.Quantity * x.Product.Price);
                orderObject = discountService.CalculateOrderDiscount(orderObject);

                var now = DateTime.UtcNow;
                orderObject.CreatedAt = now;
                orderObject.UpdatedAt = now;

                var createdOrder = await _orderRepository.Add(orderObject);

                if (createdOrder == null) { return Results.Problem("Order not created.", "", StatusCodes.Status304NotModified); }

                return Results.Created(string.Empty, _mapper.Map<OrderDto>(createdOrder));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> Delete(int id)
        {
            try
            {
                var existingOrder = await _orderRepository.GetById(id);
                if (existingOrder == null)
                    return Results.Problem("Order not found.", "", StatusCodes.Status404NotFound);

                var result = await _orderRepository.Delete(id);
                if (!result)
                    return Results.Problem("Order not deleted.", "", StatusCodes.Status304NotModified);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> GetAll()
        {
            try
            {
                var orders = await _orderRepository.GetAll();

                if (orders == null) { return Results.NotFound(); }

                return Results.Ok(_mapper.Map<List<OrderDto>>(orders));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> Update(UpdateOrderDto order)
        {
            try
            {
                var validateRequestResult = await _updateOrderDtoValidator.ValidateAsync(order);
                if (!validateRequestResult.IsValid) { return Results.ValidationProblem(validateRequestResult.ToDictionary()); }

                var orderObject = _mapper.Map<Order>(order);

                var products = await _productRepository.GetAllByIds(orderObject.OrderProducts.Select(p => p.ProductId));

                orderObject.OrderProducts = new Collection<OrderProduct>(
                                                (
                                                    from orderProduct in orderObject.OrderProducts
                                                    join product in products on orderProduct.ProductId equals product.Id
                                                    select new OrderProduct
                                                    {
                                                        Product = product,
                                                        Quantity = orderProduct.Quantity,
                                                        ProductId = orderProduct.ProductId
                                                    }).ToList()
                                                );

                var validateOrderResult = await _orderValidator.ValidateAsync(orderObject);
                if (!validateOrderResult.IsValid) { return Results.ValidationProblem(validateOrderResult.ToDictionary()); }

                var existingOrder = await _orderRepository.GetById(order.Id);
                if (existingOrder == null)
                    return Results.Problem("Order not found.", "", StatusCodes.Status404NotFound);

                var orderProductsToRemove = existingOrder.OrderProducts.Select(x => x.Id);

                existingOrder.CustomerName = orderObject.CustomerName;
                existingOrder.OrderProducts = orderObject.OrderProducts;
                existingOrder.GrossValue = orderObject.OrderProducts.Sum(x => x.Quantity * x.Product.Price);
                existingOrder = discountService.CalculateOrderDiscount(existingOrder);

                var now = DateTime.UtcNow;
                existingOrder.UpdatedAt = now;

                await _orderProductRepository.DeleteAllById(orderProductsToRemove);
                var updatedOrder = await _orderRepository.Update(existingOrder);

                if (updatedOrder == null) { return Results.Problem("Order not updated.", "", StatusCodes.Status304NotModified); }

                return Results.Ok(_mapper.Map<OrderDto>(updatedOrder));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
