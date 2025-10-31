using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PharmacyApp.Nosql.Models
{
    public class PrescriptionDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CustomerName { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
    }
}
