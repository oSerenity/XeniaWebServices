using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using XeniaWebServices.Networking.Sessions;
using XeniaWebServices.XenoAPI.Controllers; 

namespace XeniaWebServices.Controllers
{
    [ApiController]
    [Route("sessions")]
    public class SessionsController : Controller
    {

        Sessions session;
        // Inject the ILogger<T> into your controller or service
        private readonly ILogger<SessionsController> _logger;
        public SessionsController(ILogger<SessionsController> logger)
        {
            _logger = logger;
        }
        [HttpPost("sessions")]
        public IActionResult CreateSession(int titleId, [FromBody] Sessions request)
        {
            try
            {
                if (request.IsHost(request.Flags))
                {
                    Console.WriteLine("Host creating session" + request.SessionId);
                    session = Sessions.CreateSession(titleId, request.SessionId, request.HostAddress, request.Flags, request.PublicSlotsCount, request.PrivateSlotsCount, request.MacAddress, request.Port);

                    try
                    {
                        //Finds Player In Said Session.
                        Player player = Player.FindPlayer(request.HostAddress);
                        Sessions.SetPlayerSessionId(player.Xuid, request.SessionId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("BAD PLAYER " + request.HostAddress);
                    }
                }
                else
                {
                    Console.WriteLine("Peer joining session" + request.SessionId);
                    session = Sessions.GetSession(titleId, request.SessionId);
                }

                // TODO: Return an appropriate response for CreateSession.
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpPost("sessions/{sessionId}/details")]
        public IActionResult SessionDetails(int titleId, [FromBody] Sessions sessionData)
        {
            return Ok();
        }
        [HttpDelete("sessions/{sessionId}")]
        public IActionResult RemoveSession(int titleId, [FromBody] Sessions sessionData)
        {
            return Ok();
        }
        [HttpPost("sessions/{sessionId}/modify")]
        public IActionResult modifSession(int titleId, [FromBody] Sessions sessionData)
        {
            return Ok();
        }
        [HttpPost("sessions/search")]
        public IActionResult SearchSession(int titleId, [FromBody] Sessions sessionData)
        {

            return Ok();
        }
        [HttpPost("sessions/{sessionId}")]
        public IActionResult CreatexSession(int titleId, [FromBody] Sessions sessionData)
        {

            return Ok();
        }
        [HttpGet("{sessionId}/qos")]
        public IActionResult QosDownload([FromRoute(Name = "titleId")] string titleId, [FromRoute(Name = "sessionId")] string sessionId)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "qos", titleId, sessionId);

            if (!System.IO.File.Exists(path))
            {
                Response.Headers.Add("Content-Length", "0");
                return Ok(); // Assuming NotFoundException maps to HTTP 200 OK in your API
            }

            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                return NotFound(); // NotFoundException maps to HTTP 404 Not Found
            }

            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, "application/octet-stream"); // Change the content type as needed
        }

        [HttpPost("{sessionId}/qos")]
        public async Task<IActionResult> QosUpload(string titleId, string sessionId, [FromBody] string rawBody)
        {
            try
            {
                string qosPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "qos", titleId, sessionId);

                if (!Directory.Exists(qosPath))
                {
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "qos", titleId));
                    await System.IO.File.WriteAllTextAsync(qosPath, rawBody);
                }

                return Ok("QoS data uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
    public class SessionFlags
    {
        private readonly int value;

        public SessionFlags(int value)
        {
            this.value = value;
        }

        public SessionFlags Modify(SessionFlags flags)
        {
            // TODO: Implement modification logic here.
            // For now, we'll return the original flags as an example.
            Console.WriteLine("Session flag modification is not implemented!");
            return new SessionFlags(value);
        }

        public bool Advertised
        {
            get
            {
                return (value & (1 << 3)) > 0; // Equivalent to TypeScript: return this.isFlagSet(3);
            }
        }

        public bool IsHost
        {
            get
            {
                return (value & (1 << 0)) > 0; // Equivalent to TypeScript: return this.isFlagSet(0);
            }
        }
    }

}
