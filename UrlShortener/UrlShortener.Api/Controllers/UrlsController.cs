using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using UrlShortener.Api.Extensions;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services;

namespace UrlShortener.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private static readonly string UrlIdRegexPattern = $@"{AppSettingsProvider.AppDomainUrl}\/([a-zA-Z0-9]+)";
        private readonly UrlShortenerDbService _dbService;

        public UrlsController(UrlShortenerDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShortenUrl>>> Get()
        {
            var userId = await GetOrCreateUserId();

            var urls = _dbService.GetAllByUserId(userId);

            return urls;
        }

        [HttpGet]
        public async Task<ActionResult<ShortenUrl>> Create(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Parameter is required", nameof(url));
            }

            var userId = await GetOrCreateUserId();

            var shortenUrl = new ShortenUrl()
            {
                UserId = userId,
                FullUrl = url
            };

            shortenUrl = _dbService.Create(shortenUrl);
            return shortenUrl;
        }

        [HttpGet]
        public async Task<ActionResult<ShortenUrl>> GetByShortUrl(string shortUrl)
        {
            var userId = await GetOrCreateUserId();
            var urlId = GetIdFromUrl(shortUrl);
            var shortenUrl = _dbService.GetByIdAndIncViews(urlId);
            if (shortenUrl is null)
            {
                throw new MongoException("The url does not exists");
            }

            if (shortenUrl.UserId != userId)
            {
                throw new MongoException("The url does not exists");
            }

            return shortenUrl;
        }

        private async Task<ObjectId> GetOrCreateUserId()
        {
            var userName = HttpContext.User.Identity.Name;
            if (userName is null || !ObjectId.TryParse(userName, out var userId))
            {
                userId = _dbService.CreateUserId();

                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);
            }

            return userId;
        }

        private ulong GetIdFromUrl(string url)
        {
            var match = Regex.Match(url, UrlIdRegexPattern);
            if (!match.Success)
            {
                throw new ArgumentException("Wrong shorten url", nameof(url));
            }

            try
            {
                var result = match.Groups[1].Value.FromBase62();
                return result;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Wrong shorten url", nameof(url));
            }
        }
    }
}