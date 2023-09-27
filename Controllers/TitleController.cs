using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using SharpCompress.Common;
using System.Net.Http.Json;
using XeniaWebServices.Controllers;
using XeniaWebServices.Networking;
using XeniaWebServices.Networking.Sessions;

namespace XeniaWebServices.XenoAPI.Controllers
{
    [ApiController]
    [Route("title/{titleId}")]
    public class TitleController : ControllerBase
    {
        // Inject the ILogger<T> into your controller or service
        private readonly ILogger<TitleController> _logger;
        public TitleController(ILogger<TitleController> logger) 
        {
            _logger = logger;
        }
        internal int title;
        [HttpGet("servers")]
        public IActionResult Servers(string titleId)
        {
            Session.Save(titleId);
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "titles", titleId.ToUpper());
            string filePath = Path.Combine(directoryPath, "servers.json");

            if (!System.IO.File.Exists(filePath))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath);

                // Create and write the JSON content to the file
                var jsonData = new List<Servers>
            {
                new Servers
                {
                    address = "http://localhost:36000",
                    flags = 0,
                    description = "required,mass_storage,other,ttl,usr,shr,web,dbg,upl,prs,std"
                }
            };

                string jsonContent = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                System.IO.File.WriteAllText(filePath, jsonContent);

                return new JsonResult(jsonContent)
                {
                    StatusCode = 200, // Set the status code to 200 (OK) 
                    ContentType = "application/json" // Set the Content-Type header.
                };
            }

            // Read the JSON content from the file
            string jsonString = System.IO.File.ReadAllText(filePath);

            // Deserialize the JSON string into a List of Servers
            List<Servers> data = JsonConvert.DeserializeObject<List<Servers>>(jsonString);

            // Log the JSON content
            Console.WriteLine($"Servers Response Content: {jsonString}");

            // Return the JSON content as a JsonResult
            return new JsonResult(data)
            {
                StatusCode = 200, // Set the status code to 200 (OK)
                ContentType = "application/json" // Set the Content-Type header.
            };
        }
    

    [HttpGet("ports")]
        public IActionResult Ports(string titleId)
        {
            Session.Save(titleId);
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "titles", titleId.ToUpper());
            string filePath = Path.Combine(directoryPath, "ports.json");

            if (!System.IO.File.Exists(filePath))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath);

                // Create and write the JSON content to the file
                var jsonData = new
                {
                    connect = new[]
                    {
                    new { info = "Game server port.", port = 1000, mappedTo = 36010 },
                    new { info = "Game server port.", port = 1001, mappedTo = 36010 },
                    new { info = "Game server port.", port = 1002, mappedTo = 36010 }
                },
                    bind = new[]
                    {
                    new { info = "Player port 1.", port = 1000, mappedTo = 36002 },
                    new { info = "Player port 2.", port = 1001, mappedTo = 36001 }
                }
                };

                string jsonContent = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                System.IO.File.WriteAllText(filePath, jsonContent);

                return new JsonResult(jsonContent)
                {
                    StatusCode = 200, // Set the status code to 200 (OK) 
                    ContentType = "application/json" // Set the Content-Type header.
                };
            }

            // Read the JSON content from the file
            string jsonString = System.IO.File.ReadAllText(filePath);

            // Deserialize the JSON string into the Root class (assuming you have a corresponding class)
            Ports data = JsonConvert.DeserializeObject<Ports>(jsonString);

            // Log the JSON content
            Console.WriteLine($"Ports Rescontent: {jsonString}");

            // Return the JSON content as a JsonResult
            return new JsonResult(data)
            {
                StatusCode = 200, // Set the status code to 200 (OK) 
                ContentType = "application/json" // Set the Content-Type header.
            };
        }


    }
}
