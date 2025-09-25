using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace CMSAdminCrud.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles ="Admin")]
    public class PageManager : ControllerBase
    {
        private readonly ILogger<PageManager> _logger;

        public PageManager(ILogger<PageManager> logger)
        {
            _logger = logger;
        }
        public PageResult CreatePage([FromRoute(Name = "slug")] string slug)
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
