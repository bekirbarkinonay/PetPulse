using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetPulse.API.Models
{
    public class Pet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public int Age { get; set; } // Yaş eklendi
        public double Weight { get; set; } // Kilo eklendi
        public string VaccinationSchedule { get; set; } = ""; // Aşı eklendi
        public string DietaryRequirements { get; set; } = ""; // Diyet eklendi
        public string OwnerEmail { get; set; } = null!;
    }
}