using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.api.FrontolDiscountUnitApi
{
    [ApiController]
    [Route("api/fdu/[controller]")]
    public class ChequeController : ControllerBase
    {
        private readonly ILogger<ChequeController> _logger;

        public ChequeController(ILogger<ChequeController> logger)
        {
            _logger = logger;
        }

        [HttpPost("{clientid}")]
        public IActionResult ChequeWithCard(string clientid) 
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult ChequeWithoutCard()
        {
            return Ok();
        }
    }

}