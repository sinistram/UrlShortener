using MongoDB.Driver;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Services
{
    public class UrlShortenerDbService
    {
        private const long SequenceId = -1;
        private const long SequenceStart = 0;

        private readonly IMongoCollection<ShortenUrl> _urls;

        public UrlShortenerDbService(IUrlShortenerDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _urls = database.GetCollection<ShortenUrl>(settings.UrlsCollectionName);

            InitializeDb();
        }

        public ShortenUrl Create(ShortenUrl shortenUrl)
        {
            shortenUrl.Id = GetNextId();
             _urls.InsertOne(shortenUrl);
            return shortenUrl;
        }

        private long GetNextId()
        {
            var filter = Builders<ShortenUrl>.Filter.Eq(item => item.Id, SequenceId);
            var update = Builders<ShortenUrl>.Update.Inc(item => item.SequenceValue, 1);
            var url = _urls.FindOneAndUpdate(filter, update);

            if (url?.SequenceValue is null)
            {
                throw new MongoException("The database is not initialized");
            }

            return url.SequenceValue.Value;
        }

        private void InitializeDb()
        {
            if (!_urls.Find(url => url.Id == SequenceId).Any())
            {
                _urls.InsertOne(new ShortenUrl { Id = SequenceId, SequenceValue = SequenceStart });
            }
        }
    }
}