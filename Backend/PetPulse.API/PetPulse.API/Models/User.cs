using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetPulse.API.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string FirstName { get; set; } = ""; // Yeni
        public string LastName { get; set; } = "";  // Yeni
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User";
    }
}