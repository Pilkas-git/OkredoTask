using OkredoTask.Core.Entities;
using System;

namespace OkredoTask.UnitTests.EntityBuilders
{
    public static class CartBuilder
    {
        public static Cart CreateCartWithSingleItem(decimal productPrice)
        {
            var product = new Product(Guid.Empty, "product", productPrice, "description", Core.Enums.ProductType.Laptop);
            var cart = new Cart();
            cart.AddCartItem(product, 1);
            return cart;
        }
    }
}
