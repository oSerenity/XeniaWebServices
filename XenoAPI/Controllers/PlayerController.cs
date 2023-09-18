using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XeniaWebServices.Controllers;
using XeniaWebServices.XenoAPI.commands;
using XeniaWebServices.XenoAPI.queries;
using XeniaWebServices.XenoAPI.responses;

namespace XeniaWebServices.XenoAPI.Request
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
        public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(request)); // Log the request, you may need to import Newtonsoft.Json.JsonConvert


            new CreatePlayerCommand(
                new Xuid(request.Xuid),
                new Xuid(request.MachineId),
                new IpAddress(request.HostAddress),
                new MacAddress(request.MacAddress)
            );

            return Ok(); // Return appropriate response
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
                //Player(xuid, hostAddress, macAddress, machineId);
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
        public async Task<ActionResult<PlayerResponse>> FindPlayer([FromBody] FindPlayerRequest request)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(request)); // Log the request, you may need to import Newtonsoft.Json.JsonConvert

            var player = new FindPlayerQuery(new IpAddress(request.HostAddress));

            if (player == null)
            {
                _logger.LogInformation("Player not found");
            }

            var responseData = new PlayerResponse
            {
                Xuid = player.Xuid.Value,
                HostAddress = player.HostAddress.Value,
                MachineId = player.MachineId,
                Port = player.Port,
                MacAddress = player.MacAddress.Value,
                SessionId = player.SessionId.Value ?? "0000000000000000"
            };

            return Ok(responseData); // Return the response
        }
    }
}
