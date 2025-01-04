using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortner.Model;

public class UrlMapping
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [BsonElement("Key")]
    public string Key { get; set; }

    [BsonElement("Url")]
    public string Url { get; set; }

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class PostUrlModel
{
    public string Url { get; set; }
}
