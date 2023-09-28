using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using XeniaWebServices.Networking.Sessions;
using XeniaWebServices.XenoAPI.Controllers;
using XeniaWebServices.Networking;
using static XeniaWebServices.Controllers.PlayersController;

namespace XeniaWebServices.Controllers
{
    [ApiController]
    [Route("title/{titleId}/sessions")]
    public class SessionsController : Controller
    {
        Network Network { get; set; }
        // Inject the ILogger<T> into your controller or service
        private readonly ILogger<SessionsController> _logger;
        public SessionsController(ILogger<SessionsController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            Network = new Network(httpClientFactory);
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
            Session session = null;
            try
            {
                if (Session.IsHost(request.flags))
                {
                    // If the request indicates the host is creating a session
                    Console.WriteLine("Host creating session" + request.sessionId);

                    // Create a session using the provided parameters
                    session =  Session.CreateSession(
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
                    session = Session.Find(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), request.sessionId);
                }

                // Return a success response
                return Ok(session);
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
                Session session = Session.Find(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId);

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
        public async Task<IActionResult> RemoveSessionAsync(string titleId, [FromRoute(Name = "sessionId")] string sessionId)
        {
            var session = Session.Find(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId); 
            string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (clientIp == "127.0.0.1" || clientIp.StartsWith("192.168"))
            { 
                // Hi me! Who are you?
                clientIp = await Network.GetPublicIpAddressAsync();
            }

            if (session == null || session.HostAddress != clientIp)
            {
                return NotFound();
            }
            Session.DeleteSession(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId);

            return NoContent(); // 204 No Content 
        }

        [HttpGet("{sessionId}")]
        public IActionResult CurrentSession(string titleId, [FromRoute(Name = "sessionId")] string sessionId)
        {
            var session = Session.Find(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId);
            return Ok(session);
        }
        [HttpPost("{sessionId}/join")]
        public IActionResult JoinSession(string titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] Session request)
        {
            //TODO: Add Current Player ID To be Set For Joining Session
            Session.Join(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId, request.Xuid); //Do Logic
            return Ok();
        }
        [HttpPost("{sessionId}/leave")]
        public IActionResult LeaveSession(string titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] Session request)
        {
            //TODO: Add Current Player ID To be Set For Leaving Session
            Session.Leave(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId, request.Xuid); //Do Logic
            return Ok();
        }
        [HttpPost("{sessionId}/modify")]
        public IActionResult ModifySession(string titleId, [FromRoute(Name = "sessionId")] string sessionId,[FromBody] Session request)
        {
            var session = Session.Modify(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId, request.Flags, request.PublicSlotsCount, request.PrivateSlotsCount); //Do Logic
            return Ok(session);
        }
        [HttpPost("/search")]
        public IActionResult SearchSession(string titleId, [FromBody] Session request)
        {
           var session = Session.Search(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), request.SearchIndex, request.ResultsCount); //Do Logic
            return Ok(session);
        }

        [HttpPost("{sessionId}/arbitration")]
        public IActionResult Arbitration(string titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] Session request)
        {
           var session = Session.Find(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId);
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
        [HttpPost("{sessionId}/leaderboards")]
        public IActionResult Leaderboards([FromRoute(Name = "titleId")] string titleId, [FromRoute(Name = "sessionId")] string sessionId, WriteStatsRequest writeStats )
        {
            return Ok();
        }
        [HttpPost("{sessionId}/migrate")]
        public async Task<IActionResult> MigrateSession(string titleId, string sessionId, [FromBody] Session request)
        {
            var session = Session.Find(int.Parse(titleId, System.Globalization.NumberStyles.HexNumber), sessionId);

            if (session == null)
            {
                throw new NotFoundException("Session not found.");
            }

            var newSession = Session.Migrate(
                int.Parse(titleId, System.Globalization.NumberStyles.HexNumber),
                    sessionId,
                    request.HostAddress,
                    request.MacAddress,
                    request.Port
                ); 
            return Ok(newSession); // Assuming you want to return the migrated session.
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
        public class WriteStatsRequestLeaderboardStatistic
        {
            public int Type { get; set; }
            public int Value { get; set; }
        }

        public class WriteStatsRequestLeaderboard
        {
            public Dictionary<string, WriteStatsRequestLeaderboardStatistic> Stats { get; set; }
        }

        public class WriteStatsRequest
        {
            public string Xuid { get; set; }
            public Dictionary<string, WriteStatsRequestLeaderboard> Leaderboards { get; set; }
        }
    }

}
