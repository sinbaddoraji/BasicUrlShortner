using Microsoft.AspNetCore.Mvc;
using System;
using UrlShortner.Interface;
using UrlShortner.Model;

namespace UrlShortner.Controllers
{

    public class UrlShortnerController : Controller
    {
        private readonly IUrlRepository _repository;

        public UrlShortnerController(IUrlRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] PostUrlModel url)
        {
            return Ok(await _repository.SetUrlAsync(url.Url));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RedirectUrl(string id)
        {
            string url = await _repository.GetUrlAsync(id);

            if (string.IsNullOrEmpty(url))
            {
                return NotFound("The URL for the provided ID was not found.");
            }

            return Redirect(url);
        }

    }
}
