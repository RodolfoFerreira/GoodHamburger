using AutoMapper;
using FluentAssertions;
using GoodHamburger.Domain.DTO.Product;
using GoodHamburger.Infrastructure.Mapping;
using GoodHamburger.Repository.Interfaces;
using GoodHamburger.Service.Interfaces;
using GoodHamburger.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace GoodHamburger.Unit.Tests.Product
{
    using GoodHamburger.Domain.Models;

    [TestClass]
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private IMapper? _mapper;
        private IProductService? _productService;

        [TestInitialize]
        public void Setup()
        {
            // Configure AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<CategoryProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

            _productService = new ProductService(_productRepositoryMock.Object, _mapper);
        }

        [TestMethod]
        public async Task GetAllTestSuccess()
        {
            //Arrange
            var productsToReturn = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Price = 1.0, CategoryId = (int)EnumCategory.Sandwich },
                new() { Id = 2, Name = "Product 2", Price = 2.0, CategoryId = (int)EnumCategory.Garnish },
                new() { Id = 3, Name = "Product 3", Price = 3.0, CategoryId = (int)EnumCategory.Beverage }
            };

            _productRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(productsToReturn);

            //Act
            var result = await _productService!.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<ProductDto>>>();

            var finalResult = result as Ok<List<ProductDto>>;
            finalResult!.Value.Should().BeEquivalentTo(_mapper!.Map<List<ProductDto>>(productsToReturn));

            _productRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllTestException()
        {
            //Arrange
            _productRepositoryMock.Setup(x => x.GetAll()).Throws(new Exception());

            //Act
            var result = await _productService!.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _productRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllExtrasTestSuccess()
        {
            //Arrange
            var productsToReturn = new List<Product>
            {
                new() { Id = 2, Name = "Product 2", Price = 2.0, IsExtra = true, CategoryId = (int)EnumCategory.Garnish },
                new() { Id = 3, Name = "Product 3", Price = 3.0, IsExtra = true, CategoryId = (int)EnumCategory.Beverage }
            };

            _productRepositoryMock.Setup(x => x.GetAllExtras()).ReturnsAsync(productsToReturn);

            //Act
            var result = await _productService!.GetAllExtras();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<ProductDto>>>();

            var finalResult = result as Ok<List<ProductDto>>;
            finalResult!.Value.Should().BeEquivalentTo(_mapper!.Map<List<ProductDto>>(productsToReturn));

            _productRepositoryMock.Verify(x => x.GetAllExtras(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllExtrasTestException()
        {
            //Arrange
            _productRepositoryMock.Setup(x => x.GetAllExtras()).Throws(new Exception());

            //Act
            var result = await _productService!.GetAllExtras();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _productRepositoryMock.Verify(x => x.GetAllExtras(), Times.Once);
        }

        [TestMethod]
        [DataRow(EnumCategory.Sandwich)]
        [DataRow(EnumCategory.Garnish)]
        [DataRow(EnumCategory.Beverage)]
        public async Task GetAllByCategoryTestSuccess(EnumCategory category)
        {
            //Arrange
            var productsToReturn = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Price = 1.0, IsExtra = false, CategoryId = (int)category }
            };

            _productRepositoryMock.Setup(x => x.GetAllByCategory(It.IsAny<EnumCategory>())).ReturnsAsync(productsToReturn);

            //Act
            var result = await _productService!.GetAllByCategory(category);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<ProductDto>>>();

            var finalResult = result as Ok<List<ProductDto>>;
            finalResult!.Value.Should().BeEquivalentTo(_mapper!.Map<List<ProductDto>>(productsToReturn));

            _productRepositoryMock.Verify(x => x.GetAllByCategory(It.IsAny<EnumCategory>()), Times.Once);
        }

        [TestMethod]
        [DataRow(EnumCategory.Sandwich)]
        [DataRow(EnumCategory.Garnish)]
        [DataRow(EnumCategory.Beverage)]
        public async Task GetAllGetAllByCategoryTestException(EnumCategory category)
        {
            //Arrange
            _productRepositoryMock.Setup(x => x.GetAllByCategory(It.IsAny<EnumCategory>())).Throws(new Exception());

            //Act
            var result = await _productService!.GetAllByCategory(category);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProblemHttpResult>();

            var finalResult = result as ProblemHttpResult;
            finalResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _productRepositoryMock.Verify(x => x.GetAllByCategory(It.IsAny<EnumCategory>()), Times.Once);
        }
    }
}
