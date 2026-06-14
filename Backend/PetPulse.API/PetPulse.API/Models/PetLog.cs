using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetPulse.API.Models
{
    public class PetLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string PetId { get; set; } = null!;
        public string LogType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime LogDate { get; set; } = DateTime.Now;
    }
}