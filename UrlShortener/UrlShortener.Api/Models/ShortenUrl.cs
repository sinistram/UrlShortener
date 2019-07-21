using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortener.Api.Models
{
    public class ShortenUrl
    {
        [BsonId]
        public long Id { get; set; }

        [BsonElement("user"), BsonIgnoreIfNull]
        public ObjectId? UserId { get; set; }

        [BsonElement("urls"), BsonIgnoreIfNull]
        public string FullUrl { get; set; }

        [BsonElement("views"), BsonIgnoreIfNull]
        public int? ViewsCount { get; set; }

        [BsonElement("sequence_value"), BsonIgnoreIfNull]
        public long? SequenceValue { get; set; }
    }
}