using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.api
{
    [ApiController]
    [Route("[controller]")]
    public class ClientDataController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientDataController> _logger;

        public ClientDataController(IClientRepository clientRepository, ILogger<ClientDataController> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetById(string id)
        {
            var client = await _clientRepository.ByIdAsync(id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }

        [HttpGet("by-phone/{phone}")]
        public async Task<ActionResult<Client>> GetByPhone(string phone)
        {
            var client = await _clientRepository.ByPhoneAsync(phone);
            if (client == null)
                return NotFound();

            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            var result = await _clientRepository.CreateAsync(client);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Client client)
        {
            var result = await _clientRepository.UpdateAsync(client);
            if (!result)
                return NotFound();

            return Ok();
        }
    }
}
