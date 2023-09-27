using Microsoft.AspNetCore.Mvc;
using XeniaWebServices.Networking.Sessions;
using XeniaWebServices.XenoAPI.Controllers;

namespace XeniaWebServices.Controllers
{
    [ApiController]
    [Route("sessions")]
    public class SessionsController : Controller
    {
        // Inject the ILogger<T> into your controller or service
        private readonly ILogger<SessionsController> _logger;
        public SessionsController(ILogger<SessionsController> logger)
        {
            _logger = logger;
        }
        [HttpPost("sessions/{sessionId}")]
        public IActionResult CreateSoession(int titleId, [FromBody] Session sessionData)
        {
            return Ok();
        }
        [HttpPost("sessions/{sessionId}/details")]
        public IActionResult SessionDetails(int titleId, [FromBody] Session sessionData)
        {
            return Ok();
        }
        [HttpDelete("sessions/{sessionId}")]
        public IActionResult RemoveSession(int titleId, [FromBody] Session sessionData)
        {
            return Ok();
        }
        [HttpPost("sessions/{sessionId}/modify")]
        public IActionResult modifSession(int titleId, [FromBody] Session sessionData)
        {
            return Ok();
        }
        [HttpPost("sessions/search")]
        public IActionResult SearchSession(int titleId, [FromBody] Session sessionData)
        {

            return Ok();
        }
        [HttpPost("sessions")]
        public IActionResult CreateSession(int titleId, [FromBody] Session sessionData)
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
}
