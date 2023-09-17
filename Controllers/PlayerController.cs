using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using XeniaWebServices.aggregates;
using XeniaWebServices.queries;
using XeniaWebServices.responses;

namespace XeniaWebServices.Controllers
{ 
    [Route("players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(ILogger<PlayersController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> PostPlayerInfoAsync()
        {
            if (!HttpContext.Items.ContainsKey("RequestBody"))
            {
                return BadRequest("Request body not found");
            }

            var requestBody = HttpContext.Items["RequestBody"].ToString();

            try
            {
                // Parse the stored request body as JSON
                var playerInfo = JObject.Parse(requestBody);

                // Extract values from the JSON object
                var xuid = playerInfo["xuid"]?.ToString();
                var machineId = playerInfo["machineId"]?.ToString();
                string? hostAddress = playerInfo["hostAddress"]?.ToString();
                var macAddress = playerInfo["macAddress"]?.ToString();

                // Process the values as needed
                // For this example, we'll just return them in the response
                JObject jsonObject = new JObject
                {
                    ["Xuid"] = xuid,
                    ["MachineId"] = machineId,
                    ["HostAddress"] = hostAddress,
                    ["MacAddress"] = macAddress
                };

                return Ok(jsonObject);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing player info: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost("find")]
        public IActionResult FindPlayer()
        {
            if (!HttpContext.Items.ContainsKey("RequestBody"))
            {
                return BadRequest("Request body not found");
            }

            var requestBody = HttpContext.Items["RequestBody"].ToString();
            _logger.LogInformation("---Request Debug -  \r\n" + requestBody + "\r\n End---");
            try
            {
                // Parse the stored request body as JSON
                var requestData = JObject.Parse(requestBody);

                // Extract values from the JSON object
                var hostAddress = requestData["hostAddress"]?.ToString();

                // Process the values as needed
                // For this example, we'll just return them in the response
                var response = new
                {
                    HostAddress = hostAddress
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing player find request: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
