namespace UrlShortener.Api.Models
{
    public class UrlShortenerDbSettings : IUrlShortenerDbSettings
    {
        public string UrlsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IUrlShortenerDbSettings
    {
        string UrlsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}