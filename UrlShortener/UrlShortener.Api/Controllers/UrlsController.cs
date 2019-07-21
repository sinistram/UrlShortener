using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services;

namespace UrlShortener.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private readonly UrlShortenerDbService _dbService;

        public UrlsController(UrlShortenerDbService dbService)
        {
            _dbService = dbService;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShortenUrl>>> Get()
        {
            var userId = await GetOrCreateUserId();
            //_dbService.GetAllByUserId(ObjectId.Parse(userId));
            // _dbService.Create(new ShortenUrl());
            return new string[] {"value1", userId};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        private async Task<string> GetOrCreateUserId()
        {
            var userName = HttpContext.User.Identity.Name;
            if (userName is null)
            {
                userName = _dbService.CreateUserId();

                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);
            }

            return userName;
        }
    }
}