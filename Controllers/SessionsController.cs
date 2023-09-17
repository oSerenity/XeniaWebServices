using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace XeniaWebServices.Controllers
{
    public class SessionsController : ControllerBase
    {
        private readonly ILogger<SessionsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public SessionsController(ILogger<SessionsController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            _logger.LogInformation("WhoAmI Invoked");
            // You can retrieve server information here
            var serverInfo = new
            {
                ServerName = "Kestrel",
                OSVersion = Environment.OSVersion.VersionString,
                DateTime = DateTime.UtcNow
            };
            _logger.LogInformation("WhoAmI Debug - \r\n "+serverInfo+" \r\n");
            return Ok(serverInfo);
        }
        [HttpDelete("DeleteSessions")]
        public IActionResult DeleteAllSessions()
        {
            _logger.LogInformation("DeleteAllSessions Invoked");
            return Ok(); // Return an appropriate response
        }
    }
}
