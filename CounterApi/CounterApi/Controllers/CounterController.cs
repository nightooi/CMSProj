using CounterApi.Data;
using CounterApi.DataAccess;
using CounterApi.Models;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Concurrent;

namespace CounterApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class CounterController : ControllerBase
    {
        private static readonly Guid PutMagicGuid = new("7b3f9b7f-6b0c-4a8a-9b1a-0e5b2d1a9f11");
        private static readonly Guid GetMagicGuid = new("8c6a2c4e-2e3a-4d77-9d7c-5f4a3b2c1d00");

        private readonly ILogger<CounterController> _logger;
        private readonly ICounterManager _man;
        public CounterController(ILogger<CounterController> logger, ICounterManager man)
        {
            _logger = logger;
            _man = man;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async  Task<IActionResult> Increment(CounterPut input)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            if (input.Magic != PutMagicGuid)
                return Unauthorized(new { message = "Invalid token." });

            try
            {
                await _man.IncrementAsync(input.Key, HttpContext.RequestAborted);
                var val = await _man.ReadAsync(input.Key);
                var logstr = $"Incremented {input.Key} to {val}";
                _logger.LogInformation(logstr);
                return Ok(new { key = input.Key, count = val });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                return StatusCode(500, new { message = "Internal Error" });
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Read([FromQuery] CounterGet input)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            if (input.Magic != GetMagicGuid)
                return Unauthorized(new { message = "Invalid token." });

            try
            {
                    var count = await _man.ReadAsync(input.Key, HttpContext.RequestAborted);
                    if(count is not null)
                        return Ok(new { key = input.Key, count =  count});

                return NotFound(new { key = input.Key, message = "Not found." });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                return StatusCode(500, new { message = "Internal Error" });
            }
        }
    }
}
