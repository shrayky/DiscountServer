using Domain.FrontolDiscountUnit.Client.Application.Services.FduService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.api.FrontolDiscountUnitApi
{
    [ApiController]
    [Route("api/fdu/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IFduService _fduService;

        public ClientController(ILogger<ClientController> logger, IFduService fduService)
        {
            _logger = logger;
            _fduService = fduService;
        }

        [HttpGet("{identifier}")]
        public async Task<IActionResult> ClientDataAsync(string identifier)
        {
            var client = await _fduService.GetClientByIdentifierAsync(identifier);
            if (client == null)
                return NotFound();

            return Ok(client);
        } 
    }
}
