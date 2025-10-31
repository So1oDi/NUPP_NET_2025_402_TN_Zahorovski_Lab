using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PharmacyApp.Nosql.Models
{
    public class PharmacyDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
    }
}
