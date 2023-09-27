using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using XeniaWebServices.Networking;
using XeniaWebServices.Networking.Sessions; 

namespace XeniaWebServices.Controllers
{
    [ApiController]
    public class PlayersController : Controller
    {
        private readonly ILogger<PlayersController> _logger;
        public PlayersController(ILogger<PlayersController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/players")]
        public IActionResult CreatePlayer([FromBody] Player player)
        {
            if (player != null)
            {
                Sessions.AddPlayer(player.Xuid, player.MachineId, player.HostAddress, player.MacAddress);
                return Ok();

            }
            else
            {
                return BadRequest("Invalid JSON data in the request body.");
            }
        }

        [HttpPost("/players/find")]
        public IActionResult FindPlayer([FromBody] Player request)
        {
            // Find the player by host address, assuming you have a method for that.
            var player = Sessions.FindPlayerByHostAddress(request.HostAddress);

            if (player == null)
            {
                // If the player is not found, you can return a NotFound response or throw an exception as you prefer.
                return NotFound("Player not found");
            }

            // Create a PlayerResponse object based on the found player.
            var playerResponse = new PlayerResponse
            {
                xuid = player.Xuid,
                hostAddress = player.HostAddress,
                machineId = player.MachineId,
                port = player.Port, // Assuming this is correct, it should match the type expected by PlayerResponse.
                macAddress = player.MacAddress,
                sessionId = player.SessionId ?? "763c2a30975b6a29"
            };
            Console.WriteLine($"JSON Response content: \n\r {JsonConvert.SerializeObject(playerResponse)}");
            // Return the PlayerResponse as a JsonResult.
            return new JsonResult(playerResponse)
            {
                StatusCode = 200, // Set the status code to 200 (OK) if the player is found.
                ContentType = "application/json" // Set the Content-Type header.
            };
        }



        [Serializable]
        internal class NotFoundException : Exception
        {
            public NotFoundException()
            {
            }

            public NotFoundException(string? message) : base(message)
            {
                throw new Exception(message);
            }

            public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
            {
            }

            protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
