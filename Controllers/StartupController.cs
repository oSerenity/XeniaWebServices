//TODO Remake The Logic there is too many Files For Basic Logic From The Original TS Server 
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using XeniaWebServices.Networking;
using XeniaWebServices.Networking.Sessions;
using XeniaWebServices.Networking.Sessions.Manager;

namespace XeniaWebServices.XenoAPI.Controllers
{
    public class StartupController : ControllerBase
    {
        Session session { get; set; }
        private readonly ILogger<StartupController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        Network Network { get; set; }

        public StartupController(Session session, ILogger<StartupController> logger, IHttpClientFactory httpClientFactory)
        {
            this.session = session;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            Network = new Network(httpClientFactory);
        }

        [HttpGet("whoami")]
        public async Task<IActionResult> WhoAmIAsync()
        {
            IPAddress ipAddress = GetIpAddress();
            string[] splitIp = ipAddress.ToString().Split(':');
            string ipv4 = splitIp[splitIp.Length - 1];
            ipv4 = await Network.UpdateOrFetchIpAddress(ipv4);
            // Create an anonymous object to hold the ipv4 address
            var responseContent = new { address = ipv4 };

            // Log the JSON content before sending it
            Console.WriteLine($"JSON content: {JsonConvert.SerializeObject(responseContent)}");

            // Return the JSON content as a response
            return Ok(responseContent);
        }



        private IPAddress? GetIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress;
        }
         




        [HttpDelete("DeleteSessions")]
        public async Task<IActionResult> DeleteSession()
        {
            if (session == null || string.IsNullOrWhiteSpace(Session.TitleId.ToString("X")) || string.IsNullOrWhiteSpace(session.SessionId))
            {
                return Ok("Session deleted successfully.");
            }
            var retrievedSession = await SessionManager.GetSessionAsync(Session.TitleId.ToString("X"), session.SessionId);
            string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();

            if (clientIp == "::1" || clientIp.StartsWith("192.168"))
            {
                // Hi me! Who are you?
                clientIp = await Network.GetPublicIpAddressAsync();
            }
            if (retrievedSession == null || !string.Equals(retrievedSession.HostAddress, clientIp, StringComparison.OrdinalIgnoreCase))
            {
                return Ok();
            }

            // Delete the session
            await SessionManager.DeleteSessionAsync(Session.TitleId.ToString("X"), session.SessionId);

            return Ok("Session deleted successfully.");
        }

    }

}