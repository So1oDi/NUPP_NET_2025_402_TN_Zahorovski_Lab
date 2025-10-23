using System;

namespace PharmacyApp.Common
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public Customer(string name, string address)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
        }
    }
}
