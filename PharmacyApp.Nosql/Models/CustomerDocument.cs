using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PharmacyApp.Nosql.Models
{
    public class CustomerDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
