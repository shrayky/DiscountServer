using Domain.FrontolDiscountUnit.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.api.FrontolDiscountUnitApi
{
    [ApiController]
    [Route("api/fdu/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> logger;

        public TokenController(ILogger<TokenController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var tokenExpirationTime = DateTime.UtcNow.AddHours(24);

            var authAnswer = new AuthAnswer
            {
                Id = "pos",
                Name = "pos",
                Role = "operator",
                Expired = ((DateTimeOffset)tokenExpirationTime).ToUnixTimeSeconds(),
                Signature = ""
            };

            return Ok(authAnswer);
        }

    }
}
