using System;

namespace PharmacyApp.Common
{
    public class Medicine : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int QuantityInStock { get; set; }

        private static readonly Random _rnd = new Random();

        public Medicine() { }

        public Medicine(string name, double price, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            QuantityInStock = quantity;
        }

        public static Medicine CreateNew()
        {
            string[] names = { "Парацетамол", "Аспірин", "Ібупрофен", "Но-шпа", "Амоксицилін", "Лоратадин" };
            lock (_rnd)
            {
                string name = names[_rnd.Next(names.Length)];
                double price = Math.Round(5 + _rnd.NextDouble() * 45, 2);
                int qty = _rnd.Next(10, 500);
                return new Medicine(name, price, qty);
            }
        }
    }
}
