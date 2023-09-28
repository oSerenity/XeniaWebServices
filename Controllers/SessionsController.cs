using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using XeniaWebServices.Networking.Sessions;
using XeniaWebServices.XenoAPI.Controllers; 

namespace XeniaWebServices.Controllers
{
    [ApiController]
    [Route("title/{titleId}/sessions")]
    public class SessionsController : Controller
    {
         
        // Inject the ILogger<T> into your controller or service
        private readonly ILogger<SessionsController> _logger;
        public SessionsController(ILogger<SessionsController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Creates Unique Session With it's Session Information
        /// </summary>
        /// <param name="titleId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateSession(string titleId, [FromBody] SessionRequest request)
        {
            try
            {
                if (Session.IsHost(request.flags))
                {
                    // If the request indicates the host is creating a session
                    Console.WriteLine("Host creating session" + request.sessionId);

                    // Create a session using the provided parameters
                     Session.CreateSession(
                         int.Parse(titleId, System.Globalization.NumberStyles.HexNumber),
                        request.sessionId,
                        request.hostAddress,
                        request.flags,
                        request.publicSlotsCount,
                        request.privateSlotsCount,
                        request.macAddress,
                        request.port
                    );

                    try
                    {
                        // Find the player associated with the host's hostAddress
                        Player player = Player.FindPlayer(request.hostAddress, Player.Players.HostAddress);
                        // Set the player's session ID to the newly created session ID
                        player.SetPlayerSessionId(player.Xuid, request.sessionId);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("BAD PLAYER " + request.hostAddress);
                    }
                }
                else
                {
                    // If a peer is joining the session
                    Console.WriteLine("Peer joining session" + request.sessionId);

                    // Retrieve the session based on titleId and session ID
                    Session.Get(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), request.sessionId);
                }

                // Return a success response
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        // 🌈🌈🌈 This fabulous function is here to slay! Yasss queen! 💅💅💅
        [HttpGet("{sessionId}/details")]
        public async Task<ActionResult> GetSessionDetails([FromRoute] string titleId,[FromRoute] string sessionId)
        {
            try
            {
                // Call the logic to get session details based on titleId and sessionId
                Session session = Session.Get(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId);

                if (session == null)
                {
                    // If the session is not found, return a not found response
                    return NotFound("Session not found.");
                }

                // Create a SessionDetailsResponse object based on the retrieved session
                //SessionDetailsResponse response = new SessionDetailsResponse
                //{
                //    id = session.SessionId,
                //    flags = session.Flags,
                //    hostAddress = session.HostAddress,
                //    port = session.Port,
                //    macAddress = session.MacAddress,
                //    publicSlotsCount = session.PublicSlotsCount,
                //    privateSlotsCount = session.PrivateSlotsCount,
                //    openPublicSlotsCount = 0, // Replace with actual values
                //    openPrivateSlotsCount = 0, // Replace with actual values
                //    filledPublicSlotsCount = 0, // Replace with actual values
                //    filledPrivateSlotsCount = 0, // Replace with actual values
                //    players = new List<PlayerResponse>() // Replace with actual player data
                //};

                // Return the session details response
                return Ok(/*response*/);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Deletes The Session With Unique ID.
        /// </summary>
        /// <param name="titleId"></param>
        /// <param name="sessionData"></param>
        /// <returns></returns>
        [HttpDelete("{sessionId}")]
        public IActionResult RemoveSession(string titleId, [FromRoute(Name = "sessionId")] string sessionId)
        {
            Session.DeleteSession(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId);
            return Ok();
        }

        [HttpPost("join")]
        public IActionResult JoinSession(string titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] Session request)
        {
            //TODO: Add Current Player ID To be Set For Joining Session
            Session.Join(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId, request.Xuid); //Do Logic
            return Ok();
        }
        [HttpPost("leave")]
        public IActionResult LeaveSession(string titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] Session request)
        {
            //TODO: Add Current Player ID To be Set For Leaving Session
            Session.Leave(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId, request.Xuid); //Do Logic
            return Ok();
        }
        [HttpPost("{sessionId}/modify")]
        public IActionResult ModifySession(string titleId, [FromRoute(Name = "sessionId")] string sessionId,[FromBody] Session request)
        {
            Session.Modify(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId, request.Flags, request.PublicSlotsCount, request.PrivateSlotsCount); //Do Logic
            return Ok();
        }
        [HttpPost("/search")]
        public IActionResult SearchSession(string titleId, [FromBody] Session request)
        {
            Session.Search(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), request.SearchIndex, request.ResultsCount); //Do Logic
            return Ok();
        }
        [HttpPost("{sessionId}")]
        public IActionResult CurrentSession(string titleId, [FromRoute(Name = "sessionId")] string sessionId)
        {

            return Ok();
        }
        [HttpPost("{sessionId}/arbitration")]
        public IActionResult Arbitration(string titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] Session request)
        {
           var session = Session.Get(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId);
            if(session == null)
            {
                Ok("Session not found.");
            }
            else
            return Ok();
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

        public class SessionRequest
        {
            public int? flags { get; internal set; }
            public string? sessionId { get; internal set; }
            public string? hostAddress { get; internal set; }
            public int? publicSlotsCount { get; internal set; }
            public int? privateSlotsCount { get; internal set; }
            public string? macAddress { get; internal set; }
            public int? port { get; internal set; }
        }
    }

    /// <summary>
    /// TODO: Delete This Once Fully Intergrated we want to use less files aka classes bieng made just to handle simple logic this was made as a place holder logic idea..
    /// </summary>
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
