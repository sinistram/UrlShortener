using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using UrlShortener.Api.Extensions;

namespace UrlShortener.Api.Models
{
    public class ShortenUrl
    {
        [BsonId, JsonIgnore]
        public ulong Id { get; set; }

        [BsonElement("user"), JsonIgnore]
        public ObjectId UserId { get; set; }

        [BsonElement("url")]
        public string FullUrl { get; set; }

        [BsonElement("views")]
        public int ViewsCount { get; set; }

        [BsonElement("sequence_value"), BsonIgnoreIfNull, JsonIgnore]
        public ulong? SequenceValue { get; set; }

        [BsonIgnore] public string ShortUrl => $"{AppSettingsProvider.AppDomainUrl}/{Id.ToBase62()}";
    }
}