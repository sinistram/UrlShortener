using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Services
{
    public class UrlShortenerDbService
    {
        private const ulong SequenceId = 0;
        private const ulong SequenceStart = 1;

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

        public ShortenUrl GetByIdAndIncViews(ulong id)
        {
            var filter = Builders<ShortenUrl>.Filter.Eq(item => item.Id, id);
            var update = Builders<ShortenUrl>.Update.Inc(item => item.ViewsCount, 1);
            var url = _urls.FindOneAndUpdate(filter, update);

            if (url is null)
            {
                throw new ArgumentException($"The url with id {id} not found.", nameof(id));
            }

            return url;
        }

        public List<ShortenUrl> GetAllByUserId(ObjectId userId)
        {
            return _urls.Find(url => url.UserId == userId).ToList();
        }

        public string CreateUserId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        private ulong GetNextId()
        {
            var filter = Builders<ShortenUrl>.Filter.Eq(item => item.Id, SequenceId);
            var update = Builders<ShortenUrl>.Update.Inc(item => item.SequenceValue, (ulong)1);
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