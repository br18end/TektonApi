using Ardalis.GuardClauses;

namespace TektonApi.Data.Entities
{
    public class Product
    {
        //Private set to allow encapsulation
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }

        public Product(string name, string? description, decimal price)
        {
            //Preventing name and price from null, negative or zero
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NegativeOrZero(price, nameof(price));

            Name = name;
            Description = description;
            Price = price;
        }

        public void Update(string name, string? description, decimal price)
        {
            //Preventing name and price from null, negative or zero
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NegativeOrZero(price, nameof(price));

            Name = name;
            Description = description;
            Price = price;
        }
    }
}
