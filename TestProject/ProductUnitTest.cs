using System;
using TektonApi.Data.Entities;
using Xunit;

namespace TestProject
{
    public class ProductUnitTest
    {
        [Fact]
        public void ShouldNotCreateProductGivenEmptyName()
        {
            Assert.Throws<ArgumentException>(() => new Product(string.Empty, string.Empty, 5.99M));
        }

        [Fact]
        public void ShouldNotCreateProductGivenZeroPrice()
        {
            Assert.Throws<ArgumentException>(() => new Product("Orange juice", string.Empty, 0));
        }
    }
}