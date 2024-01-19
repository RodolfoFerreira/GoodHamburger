using AutoMapper;
using GoodHamburger.Domain.DTO.Order;
using GoodHamburger.Domain.DTO.OrderProduct;
using GoodHamburger.Infrastructure.Mapping;
using GoodHamburger.Infrastructure.Validators;
using GoodHamburger.Repository.Interfaces;
using GoodHamburger.Service.Interfaces;
using GoodHamburger.Service.Services;
using Moq;

namespace GoodHamburger.Unit.Tests.Order
{
    using FluentAssertions;
    using GoodHamburger.Domain.DTO.Product;
    using GoodHamburger.Domain.Models;
    using GoodHamburger.Repository.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;

    [TestClass]
    public class OrderServiceTest
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly Mock<IOrderProductRepository> _orderProductRepositoryMock = new();
        private IMapper? _mapper;
        private IOrderService? _orderService;

        [TestInitialize]
        public void Setup()
        {
            // Configure AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrderProfile>();
                cfg.AddProfile<OrderProductProfile>();
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<CategoryProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

            _orderService = new OrderService(
                _orderRepositoryMock.Object,
                _productRepositoryMock.Object,
                _orderProductRepositoryMock.Object,
                new DiscountService(),
                new CreateOrderDtoValidator(),
                new UpdateOrderDtoValidator(),
                new OrderValidator(),
                _mapper
            );
        }

        [TestMethod]
        public async Task AddTestSuccess()
        {
            //Arrange
            CreateOrderDto createOrder = new()
            {
                CustomerName = "Teste",
                OrderProducts = [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 }
                ]
            };

            Order order = new()
            {
                Id = 1,
                CustomerName = "Teste",
                GrossValue = 10,
                NetValue = 10,
                DiscountPerc = 0,
                DiscountValue = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrderProducts = [
                    new OrderProduct { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1 }
                ],
            };

            var products = new List<Product> {
                new () { Id = 1, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Price = 10 }
            };

            _productRepositoryMock.Setup(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>())).ReturnsAsync(products);
            _orderRepositoryMock.Setup(x => x.Add(It.IsAny<Order>())).ReturnsAsync(order);

            //Act
            var result = await _orderService!.Add(createOrder);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Created<OrderDto>>();

            var finalResult = result as Created<OrderDto>;
            finalResult!.Value.Should().BeEquivalentTo(_mapper!.Map<OrderDto>(order));

            _productRepositoryMock.Verify(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
        }

        [TestMethod]
        public async Task AddTestErrorRepeatedItems()
        {
            //Arrange
            CreateOrderDto createOrder = new()
            {
                CustomerName = "Teste",
                OrderProducts = [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 },
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 }
                ]
            };

            //Act
            var result = await _orderService!.Add(createOrder);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            _productRepositoryMock.Verify(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>()), Times.Never);
            _orderRepositoryMock.Verify(x => x.Add(It.IsAny<Order>()), Times.Never);
        }

        [TestMethod]
        public async Task AddTestErrorCombinatedItems()
        {
            //Arrange
            CreateOrderDto createOrder = new()
            {
                CustomerName = "Teste",
                OrderProducts = [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 },
                    new CreateOrderProductDto { ProductId = 2, Quantity = 1 }
                ]
            };

            var products = new List<Product> {
                new () { Id = 1, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Price = 10 },
                new () { Id = 1, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Price = 10 },
            };

            _productRepositoryMock.Setup(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>())).ReturnsAsync(products);

            //Act
            var result = await _orderService!.Add(createOrder);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            _productRepositoryMock.Verify(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.Add(It.IsAny<Order>()), Times.Never);
        }

        [TestMethod]
        public async Task DeleteTestSuccess()
        {
            //Arrange
            var existingOrder = new Order { Id = 1 };

            _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingOrder);
            _orderRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var result = await _orderService!.Delete(1);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok>();

            _orderRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task DeleteTestNotFound()
        {
            //Arrange
            _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Order?)null);

            //Act
            var result = await _orderService!.Delete(1);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            _orderRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task DeleteTestException()
        {
            //Arrange
            _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Throws(new Exception());

            //Act
            var result = await _orderService!.Delete(1);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _orderRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetAllTestSuccess()
        {
            //Arrange
            var ordersToReturn = new List<Order>
            {
                new() { Id = 1 },
                new() { Id = 2 },
                new() { Id = 3 }
            };

            _orderRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(ordersToReturn);

            //Act
            var result = await _orderService!.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<OrderDto>>>();

            var finalResult = result as Ok<List<OrderDto>>;
            finalResult!.Value.Should().BeEquivalentTo(_mapper!.Map<List<OrderDto>>(ordersToReturn));

            _orderRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllTestException()
        {
            //Arrange
            _orderRepositoryMock.Setup(x => x.GetAll()).Throws(new Exception());

            //Act
            var result = await _orderService!.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _orderRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [TestMethod]
        public async Task UpdateSuccess()
        {
            //Arrange
            UpdateOrderDto updateOrder = new()
            {
                Id = 1,
                CustomerName = "Teste",
                OrderProducts = [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 }
                ]
            };

            Order order = new()
            {
                Id = 1,
                CustomerName = "Teste",
                GrossValue = 10,
                NetValue = 10,
                DiscountPerc = 0,
                DiscountValue = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrderProducts = [
                    new OrderProduct { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1 }
                ],
            };

            var products = new List<Product> {
                new () { Id = 1, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Price = 10 }
            };

            _productRepositoryMock.Setup(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>())).ReturnsAsync(products);
            _orderRepositoryMock.Setup(x => x.Update(It.IsAny<Order>())).ReturnsAsync(order);
            _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(order);
            _orderProductRepositoryMock.Setup(x => x.DeleteAllById(It.IsAny<IEnumerable<int>>())).ReturnsAsync(updateOrder.OrderProducts.Count);

            //Act
            var result = await _orderService!.Update(updateOrder);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<OrderDto>>();

            var finalResult = result as Ok<OrderDto>;
            finalResult!.Value.Should().BeEquivalentTo(_mapper!.Map<OrderDto>(order));

            _productRepositoryMock.Verify(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _orderProductRepositoryMock.Verify(x => x.DeleteAllById(It.IsAny<IEnumerable<int>>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateErrorRepeatedItems()
        {
            //Arrange
            UpdateOrderDto updateOrder = new()
            {
                Id = 1,
                CustomerName = "Teste",
                OrderProducts = [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 },
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 }
                ]
            };

            //Act
            var result = await _orderService!.Update(updateOrder);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            _productRepositoryMock.Verify(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>()), Times.Never);
            _orderRepositoryMock.Verify(x => x.Update(It.IsAny<Order>()), Times.Never);
            _orderRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);
            _orderProductRepositoryMock.Verify(x => x.DeleteAllById(It.IsAny<IEnumerable<int>>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateErrorCombinedItems()
        {
            //Arrange
            UpdateOrderDto updateOrder = new()
            {
                Id = 1,
                CustomerName = "Teste",
                OrderProducts = [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 },
                    new CreateOrderProductDto { ProductId = 2, Quantity = 1 }
                ]
            };

            var products = new List<Product> {
                new () { Id = 1, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Price = 10 },
                new () { Id = 1, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Price = 10 },
            };

            _productRepositoryMock.Setup(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>())).ReturnsAsync(products);

            //Act
            var result = await _orderService!.Update(updateOrder);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            _productRepositoryMock.Verify(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.Update(It.IsAny<Order>()), Times.Never);
            _orderRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);
            _orderProductRepositoryMock.Verify(x => x.DeleteAllById(It.IsAny<IEnumerable<int>>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateErrorException()
        {
            //Arrange
            UpdateOrderDto updateOrder = new()
            {
                Id = 1,
                CustomerName = "Teste",
                OrderProducts = [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 }
                ]
            };

            _productRepositoryMock.Setup(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>())).Throws(new Exception());

            //Act
            var result = await _orderService!.Update(updateOrder);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _productRepositoryMock.Verify(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.Update(It.IsAny<Order>()), Times.Never);
            _orderRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);
            _orderProductRepositoryMock.Verify(x => x.DeleteAllById(It.IsAny<IEnumerable<int>>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateErrorNotFound()
        {
            //Arrange
            UpdateOrderDto updateOrder = new()
            {
                Id = 1,
                CustomerName = "Teste",
                OrderProducts = [
                    new CreateOrderProductDto { ProductId = 1, Quantity = 1 }
                ]
            };

            var products = new List<Product> {
                new () { Id = 1, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Price = 10 },
            };

            _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Order?)null);
            _productRepositoryMock.Setup(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>())).ReturnsAsync(products);

            //Act
            var result = await _orderService!.Update(updateOrder);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            _productRepositoryMock.Verify(x => x.GetAllByIds(It.IsAny<IEnumerable<int>>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.Update(It.IsAny<Order>()), Times.Never);
            _orderRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _orderProductRepositoryMock.Verify(x => x.DeleteAllById(It.IsAny<IEnumerable<int>>()), Times.Never);
        }
    }
}
