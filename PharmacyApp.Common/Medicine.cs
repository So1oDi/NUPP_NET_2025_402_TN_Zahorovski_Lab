using System;

namespace PharmacyApp.Common
{
    public class Medicine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int QuantityInStock { get; set; }

        public Medicine(string name, double price, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            QuantityInStock = quantity;
        }
    }
}
