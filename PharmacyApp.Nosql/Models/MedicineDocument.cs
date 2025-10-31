using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PharmacyApp.Nosql.Models
{
    public class MedicineDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")] // поле в MongoDB
        public string Name { get; set; } = string.Empty;

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("quantityInStock")]
        public int QuantityInStock { get; set; }
    }
}
