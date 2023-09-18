using Microsoft.AspNetCore.Mvc;
using System.Net;
using XeniaWebServices.Controllers;
using XeniaWebServices.XenoAPI.aggregates;
using XeniaWebServices.XenoAPI.commands;
using XeniaWebServices.XenoAPI.Request;
using XeniaWebServices.XenoAPI.responses;

namespace XeniaWebServices.XenoAPI.Controllers
{
    public class SessionsController : ControllerBase
    {
        internal IpAddress? hostAddress { get; set; }
        private readonly ILogger<SessionsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public SessionsController(ILogger<SessionsController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public ActionResult<SessionDetailsResponse> GetSessionDetails([FromRoute(Name = "titleId")] string titleId, [FromRoute(Name = "sessionId")] string sessionId)
        {
            var session = new GetSessionQuery(new TitleId(titleId), new SessionId(sessionId));

            if (session == null)
            {
                return NotFound("Session not found.");
            }
            // Create and return the SessionDetailsResponse
            var response = new SessionDetailsResponse
            {
                Id = session.Id,
                Flags = session.Flags,
                HostAddress = session.HostAddress,
                Port = session.Port,
                MacAddress = session.MacAddress,
                PublicSlotsCount = session.PublicSlotsCount,
                PrivateSlotsCount = session.PrivateSlotsCount,
                OpenPublicSlotsCount = session.OpenPublicSlots,
                OpenPrivateSlotsCount = session.OpenPrivateSlots,
                FilledPublicSlotsCount = session.FilledPublicSlots,
                FilledPrivateSlotsCount = session.FilledPrivateSlots,
                Players = session.Players.Select(xuid => new Player(xuid.Value)).ToList()


            };

            return Ok(response);
        }
        [HttpGet("whoami")]
        public async Task<IActionResult> WhoAmIAsync()
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            if (IPAddress.IsLoopback(HttpContext.Connection.RemoteIpAddress) ||
                IPAddress.Parse("192.168.0.0").IsIPv4MappedToIPv6 ||
                IPAddress.Parse("10.0.0.0").IsIPv4MappedToIPv6)
            {
                var httpClient = _httpClientFactory.CreateClient();

                // Make an external request to obtain the public IP address
                var response = await httpClient.GetAsync("https://api.ipify.org/");
                ipAddress = await response.Content.ReadAsStringAsync();
                hostAddress = new IpAddress(ipAddress);
            }

            return Ok(new { address = ipAddress });
        }
        [HttpPost]
        [Route("{titleId}sessionId/qos")]
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



        [HttpGet]
        [Route("{titleId}/sessionId/qos")]
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

        [HttpDelete("DeleteSessions")]
        public async Task<IActionResult> DeleteAllSessions()
        {
            string ipv4 = HttpContext.Connection.RemoteIpAddress.ToString();
            if (hostAddress == null)
            {
                hostAddress = new IpAddress("");
            }
            if (!string.IsNullOrEmpty(hostAddress.Value))
            {
                ipv4 = hostAddress.Value;
                if (IPAddress.TryParse(ipv4, out var ipAddress) &&
                   (IPAddress.IsLoopback(ipAddress) ||
                    ipAddress.ToString().StartsWith("192.168") ||
                    ipAddress.ToString().StartsWith("10")))
                {
                    // Make an external request to obtain the public IP address
                    var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync("https://api.ipify.org/");
                    ipv4 = await response.Content.ReadAsStringAsync();
                }

                // Create a new DeleteSessionCommand with null Title and SessionId
                commands.DeleteSessionCommand deleteSessionCommand = new commands.DeleteSessionCommand(null, null);
                //deleteSessionCommand.
            }



            return Ok();
        }
        [HttpPost("{sessionId}/join")]
        public async Task<IActionResult> JoinSession([FromRoute(Name = "titleId")] string titleId, [FromRoute(Name = "sessionId")] string sessionId, [FromBody] JoinSessionRequest request)
        {
            // Assuming request.Xuids is a List<Xuid> or an array of Xuid
            var xuids = request.Xuids.ToList(); // Convert to List<Xuid> if needed

            var joinCommand = new JoinSessionCommand(new TitleId(titleId), new SessionId(sessionId), xuids);

            return Ok(); // Return appropriate response
        }

        [HttpPost("{sessionId}/leave")]
        public async Task<IActionResult> LeaveSession(
            [FromRoute(Name = "titleId")] string titleId,
            [FromRoute(Name = "sessionId")] string sessionId,
            [FromBody] LeaveSessionRequest request)
        {
            // new LeaveSessionCommand(new TitleId(titleId),new SessionId(sessionId),request.Xuids.Select(xuid => new Xuid(xuid)).ToList());

            return Ok(); // Return appropriate response
        }

        [HttpPost("search")]
        public async Task<IActionResult> SessionSearch(
            [FromRoute(Name = "titleId")] string titleId,
            [FromBody] SessionSearchRequest request)
        {
            // Assuming you have a service or method to retrieve sessions
            //var sessions = await _sessionService.GetSessionsAsync(new TitleId(titleId), request.SearchIndex, request.ResultsCount);

            //var presentationModels = sessions.Select(session => _sessionMapper.MapToPresentationModel(session)).ToList();

            return Ok(/*presentationModels*/);
        }

    }

}
public class SessionDto
{
    public string Id { get; set; }
    public string HostAddress { get; set; }
    public string MacAddress { get; set; }
    public int PublicSlotsCount { get; set; }
    public int PrivateSlotsCount { get; set; }
    public int OpenPublicSlotsCount { get; set; }
    public int OpenPrivateSlotsCount { get; set; }
    public int FilledPublicSlotsCount { get; set; }
    public int FilledPrivateSlotsCount { get; set; }
    public int Port { get; set; }
    public XeniaWebServices.value_objects.Flags Flags { get; internal set; }
}
public class SessionPresentationMapper
{
    private readonly ILogger _logger; // Replace with your actual logger interface

    public SessionPresentationMapper(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public SessionDto MapToPresentationModel(Session session)
    {
        if (session == null)
        {
            throw new ArgumentNullException(nameof(session));
        }

        return new SessionDto
        {
            Id = session.Id.Value,
            Flags = session.Flags,
            HostAddress = session.HostAddress.Value,
            MacAddress = session.MacAddress.Value,
            PublicSlotsCount = session.PublicSlotsCount,
            PrivateSlotsCount = session.PrivateSlotsCount,
            OpenPublicSlotsCount = session.OpenPublicSlots,
            OpenPrivateSlotsCount = session.OpenPrivateSlots,
            FilledPublicSlotsCount = session.FilledPublicSlots,
            FilledPrivateSlotsCount = session.FilledPrivateSlots,
            Port = session.Port
        };
    }
}