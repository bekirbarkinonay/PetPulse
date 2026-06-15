using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Email")]    // büyük E
public string Email { get; set; }

[BsonElement("Password")] // büyük P
public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}