using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortener.Api.Models
{
    public class ShortenUrl
    {
        [BsonId]
        public ulong Id { get; set; }

        [BsonElement("user")]
        public ObjectId UserId { get; set; }

        [BsonElement("url")]
        public string FullUrl { get; set; }

        [BsonElement("views")]
        public int ViewsCount { get; set; }

        [BsonElement("sequence_value"), BsonIgnoreIfNull]
        public ulong? SequenceValue { get; set; }
    }
}