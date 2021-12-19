using NUnit.Framework;
using OkredoTask.Core.Entities;
using OkredoTask.Core.Exceptions;
using OkredoTask.UnitTests.EntityBuilders;
using Shouldly;
using System;

namespace OkredoTask.UnitTests
{
    public class CartTests
    {
        [Test]
        public void RemoveCartItem_ReturnsCorrectPrice()
        {
            //Arrange
            var productPrice = 14;
            var productQuantity = 2;
            var correctPrice = 0;

            var product = new Product(Guid.Empty, "product", productPrice, "description", Core.Enums.ProductType.Laptop);
            var cart = new Cart();

            cart.AddCartItem(product, productQuantity);

            //Act
            cart.RemoveCartItem(product.ProductId);

            //Assert
            cart.Price.ShouldBe(correctPrice);
        }

        [Test]
        public void RemoveCartItem_BadProductId_ThrowsDomainException()
        {
            //Arrange
            var cart = new Cart();

            //Act
            //Assert
            Should.Throw<DomainException>(() => cart.RemoveCartItem(Guid.Empty));
        }

        [Test]
        [TestCase(14, 1, 14)]
        [TestCase(14, 2, 28)]
        [TestCase(14, 0, 0)]
        public void AddCartItem_ReturnsCorrectCartPrice(decimal productPrice, int quantity, decimal correctPrice)
        {
            //Arrange
            var product = new Product(Guid.Empty, "product", productPrice, "description", Core.Enums.ProductType.Laptop);
            var cart = new Cart();

            //Act
            cart.AddCartItem(product, quantity);

            //Assert
            cart.Price.ShouldBe(correctPrice);
        }

        [Test]
        public void ApplyDiscountCode_FixedDiscount_ReturnsCorrectCartPrice()
        {
            //Arrange
            var productPrice = 14;
            var fixedDiscountValue = 10;
            var correctPrice = 4;
            var cart = CartBuilder.CreateCartWithSingleItem(productPrice);
            var discount = new Discount("discountCode", fixedDiscountValue, true, true, new DateTime(9999,9,9));

            //Act
            cart.ApplyDiscountCode(discount);

            //Assert
            cart.Price.ShouldBe(correctPrice);
        }

        [Test]
        public void ApplyDiscountCode_NonFixedDiscount_ReturnsCorrectCartPrice()
        {
            //Arrange
            var productPrice = 14;
            var discountValue = 0.1m;
            var correctPrice = 12.6m;
            var cart = CartBuilder.CreateCartWithSingleItem(productPrice);
            var discount = new Discount("discountCode", discountValue, true, false, new DateTime(9999, 9, 9));

            //Act
            cart.ApplyDiscountCode(discount);

            //Assert
            cart.Price.ShouldBe(correctPrice);
        }

        [Test]
        [TestCase(14, 20.44, 1, 20.44)]
        [TestCase(14, 20.44, 2, 40.88)]
        [TestCase(14, 20.44, 0, 0)]
        public void UpdateCartPrice_ChangedProductPrice_ReturnsCorrectCartPrice(decimal productPrice, decimal newProductPrice, int quantity, decimal correctPrice)
        {
            //Arrange
            var product = new Product(Guid.Empty, "product", productPrice, "description", Core.Enums.ProductType.Laptop);
            var cart = new Cart();
            cart.AddCartItem(product, quantity);
            product.UpdateProductDetails(product.Name, newProductPrice, product.Description, product.AvailabilityCount);

            //Act
            cart.UpdateCartPrice();

            //Assert
            cart.Price.ShouldBe(correctPrice);
        }
    }
}