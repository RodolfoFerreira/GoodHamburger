namespace GoodHamburger.Unit.Tests.Discount
{
    using FluentAssertions;
    using GoodHamburger.Domain.Models;
    using GoodHamburger.Service.Interfaces;
    using GoodHamburger.Service.Services;

    [TestClass]
    public class DiscountServiceTest
    {

        private IDiscountService? _discountService;

        [TestInitialize]
        public void Setup()
        {
            _discountService = new DiscountService();
        }

        [TestMethod]
        [DataRow(EnumDiscountTestType.Perc0)]
        [DataRow(EnumDiscountTestType.Perc10)]
        [DataRow(EnumDiscountTestType.Perc15)]
        [DataRow(EnumDiscountTestType.Perc20)]
        public void CalculateOrderDiscountTest(EnumDiscountTestType discountType)
        {
            //Assert
            var dictOrder = new Dictionary<EnumDiscountTestType, Order>() {
                {
                    EnumDiscountTestType.Perc0,
                    new Order ()
                    {
                        GrossValue = 100,
                        OrderProducts = [
                            new() { Quantity = 1, Product = new Product { CategoryId = (int)EnumCategory.Sandwich } }
                        ]
                    }
                },
                {
                    EnumDiscountTestType.Perc10,
                    new Order ()
                    {
                        GrossValue = 100,
                        OrderProducts = [
                            new() { Quantity = 1, Product = new Product { CategoryId = (int)EnumCategory.Sandwich } },
                            new() { Quantity = 1, Product = new Product { CategoryId = (int)EnumCategory.Garnish } }
                        ]
                    }
                },
                {
                    EnumDiscountTestType.Perc15,
                    new Order ()
                    {
                        GrossValue = 100,
                        OrderProducts = [
                            new() { Quantity = 1, Product = new Product { CategoryId = (int)EnumCategory.Sandwich } },
                            new() { Quantity = 1, Product = new Product { CategoryId = (int)EnumCategory.Beverage } }
                        ]
                    }
                },
                {
                    EnumDiscountTestType.Perc20,
                    new Order ()
                    {
                        GrossValue = 100,
                        OrderProducts = [
                            new() { Quantity = 1, Product = new Product { CategoryId = (int)EnumCategory.Sandwich } },
                            new() { Quantity = 1, Product = new Product { CategoryId = (int)EnumCategory.Garnish } },
                            new() { Quantity = 1, Product = new Product { CategoryId = (int)EnumCategory.Beverage } }
                        ]
                    }
                }
            };

            var dictDiscount = new Dictionary<EnumDiscountTestType, int>() {
                { EnumDiscountTestType.Perc0, 0 },
                { EnumDiscountTestType.Perc10, 10 },
                { EnumDiscountTestType.Perc15, 15 },
                { EnumDiscountTestType.Perc20, 20 },
            };

            var dictNetValue = new Dictionary<EnumDiscountTestType, int>() {
                { EnumDiscountTestType.Perc0, 100 },
                { EnumDiscountTestType.Perc10, 90 },
                { EnumDiscountTestType.Perc15, 85 },
                { EnumDiscountTestType.Perc20, 80 },
            };

            //Arrange
            var result = _discountService!.CalculateOrderDiscount(dictOrder[discountType]);

            //Assert
            result.Should().NotBeNull();
            result.DiscountPerc.Should().Be(dictDiscount[discountType]);
            result.DiscountValue.Should().Be(dictDiscount[discountType]);
            result.NetValue.Should().Be(dictNetValue[discountType]);
        }
    }

    public enum EnumDiscountTestType
    {
        Perc20,
        Perc15,
        Perc10,
        Perc0
    }
}
