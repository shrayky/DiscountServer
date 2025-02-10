namespace API.Controllers.api
{
    using Microsoft.AspNetCore.Mvc;
    using Shared.Configuration;
    using Shared.Configuration.interfaces;

    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpGet]
        public ActionResult<Dictionary<string, string>> GetAllParameters()
        {
            var parameters = _configurationService.GetSettings();
            return Ok(parameters);
        }

        [HttpPost]
        public ActionResult UpdateParameters([FromBody] AppSettings settings)
        {
            try
            {
                _configurationService.SaveConfiguration(settings);
                
                return Ok("Parameters updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update parameters: {ex.Message}");
            }
        }
    }
}
