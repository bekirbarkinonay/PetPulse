using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("email")] // Veritabanında her zaman küçük harf 'email' olarak sakla
    public string Email { get; set; }

    [BsonElement("password")] // Veritabanında her zaman küçük harf 'password' olarak sakla
    public string Password { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}