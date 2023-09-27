﻿using Amazon.Runtime.Internal;
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

        Session session;
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
        [HttpPost("sessions")]
        public IActionResult CreateSession(int titleId, [FromBody] Session request)
        {
            try
            {
                if (request.IsHost(request.Flags))
                {
                    // If the request indicates the host is creating a session
                    Console.WriteLine("Host creating session" + request.SessionId);

                    // Create a session using the provided parameters
                    session = Session.CreateSession(
                        titleId,
                        request.SessionId,
                        request.HostAddress,
                        request.Flags,
                        request.PublicSlotsCount,
                        request.PrivateSlotsCount,
                        request.MacAddress,
                        request.Port
                    );

                    try
                    {
                        // Find the player associated with the host's hostAddress
                        Player player = Player.FindPlayer(request.HostAddress, Player.Players.HostAddress);
                        

                        // Set the player's session ID to the newly created session ID
                        Player.SetPlayerSessionId(player.Xuid, request.SessionId);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("BAD PLAYER " + request.HostAddress);
                    }
                }
                else
                {
                    // If a peer is joining the session
                    Console.WriteLine("Peer joining session" + request.SessionId);

                    // Retrieve the session based on titleId and session ID
                    session = Session.Get(titleId, request.SessionId);
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
        [HttpGet("/{sessionId}/details")]
        public async Task<ActionResult> GetSessionDetails([FromRoute] string titleId,[FromRoute] string sessionId)
        {
            try
            {
                // Call the logic to get session details based on titleId and sessionId
                Session session = Session.Get(int.Parse(titleId), sessionId);

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
        [HttpDelete("sessions/{sessionId}")]
        public IActionResult RemoveSession(int titleId, [FromRoute(Name = "sessionId")] string sessionId)
        {
            Session.DeleteSession(titleId, sessionId);
            return Ok();
        }

        [HttpPost("sessions/{sessionId}/join")]
        public IActionResult JoinSession(int titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] Session request)
        {
            //TODO: Add Current Player ID To be Set For Joining Session
            Session.Join(titleId, sessionId, request._Xuid); //Do Logic
            return Ok();
        }
        [HttpPost("sessions/{sessionId}/leave")]
        public IActionResult LeaveSession(int titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] Session request)
        {
            //TODO: Add Current Player ID To be Set For Leaving Session
            Session.Leave(titleId, sessionId, request._Xuid); //Do Logic
            return Ok();
        }
        [HttpPost("sessions/{sessionId}/modify")]
        public IActionResult modifySession(int titleId, [FromRoute(Name = "sessionId")] string sessionId,[FromBody] Session request)
        {
            Session.Modify(titleId, sessionId, request.Flags, request.PublicSlotsCount, request.PrivateSlotsCount); //Do Logic
            return Ok();
        }
        [HttpPost("sessions/search")]
        public IActionResult SearchSession(int titleId, [FromBody] Session request)
        {
            Session.Search(titleId, request.SearchIndex, request.ResultsCount); //Do Logic
            return Ok();
        }
        [HttpPost("sessions/{sessionId}")]
        public IActionResult CurrentSession(int titleId, [FromRoute(Name = "sessionId")] string sessionId)
        {

            return Ok();
        }
        [HttpPost("sessions/{sessionId}/arbitration")]
        public IActionResult Arbitration(int titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] Session request)
        {
           var session = Session.Get(titleId, sessionId);
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
