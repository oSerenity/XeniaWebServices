using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;


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
        } //_logger.LogInformation("WhoAmI Invoked");
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
            }

            return Ok(new { address = ipAddress });
        }
    
        [HttpDelete("DeleteSessions")]
        public async Task<IActionResult> DeleteAllSessions([FromQuery] string hostAddress)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.ToString();

            if (!string.IsNullOrEmpty(hostAddress))
            {
                ip = hostAddress;
            }

            if (IPAddress.TryParse(ip, out var ipAddress) &&
                (IPAddress.IsLoopback(ipAddress) ||
                 ipAddress.ToString().StartsWith("192.168") ||
                 ipAddress.ToString().StartsWith("10")))
            {
                // Make an external request to obtain the public IP address
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync("https://api.ipify.org/");
                ip = await response.Content.ReadAsStringAsync();
            }

            // Execute the DeleteSessionCommand with the obtained IP address (ip)
            new DeleteSessionCommand(null, null, new IpAddress(ip));

            return Ok();
        }
    }
}
