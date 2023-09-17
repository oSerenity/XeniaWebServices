using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XeniaWebServices.Controllers
{
    [ApiController]
    [Route("title/{titleId}")]
    public class TitleController : ControllerBase
    {
        [HttpGet("servers")]
        public IActionResult Servers(string titleId)
        {
            TittleID.TitleID = titleId;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "titles", titleId.ToUpper(), "servers.json"
            );

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            FileInfo fileInfo = new FileInfo(path);

            HttpContext.Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            return new FileStreamResult(System.IO.File.OpenRead(path), "application/json");
        }
        [HttpGet("ports")]
        public IActionResult Ports(string titleId)
        {
            TittleID.TitleID = titleId;
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "titles",
                titleId.ToUpper(),
                "ports.json"
            );

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            FileInfo fileInfo = new FileInfo(path);

            HttpContext.Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            return new FileStreamResult(System.IO.File.OpenRead(path), "application/json");
        }
    }
}
