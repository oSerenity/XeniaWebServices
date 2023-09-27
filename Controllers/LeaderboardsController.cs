using Microsoft.AspNetCore.Mvc;
using System;
using XeniaWebServices.Networking.Sessions;

namespace XeniaWebServices.Controllers
{
    [ApiController]
    [Route("leaderboards")]
    public class LeaderboardsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("leaderboards/find")] 
        public ActionResult<FindLeaderboardsResponse> FindPlayer([FromBody] LeaderboardsRequest request)
        {
            try
            {
                // Log the incoming request for debugging purposes
                Console.WriteLine(request);

                // Create an empty response
                var response = new FindLeaderboardsResponse();

                // Process the request data and populate the response manually
                foreach (var player in request.Players)
                {
                    var leaderboardPlayer = new LeaderboardPlayer
                    {
                        Xuid = player,
                        Gamertag = "YourGamertag", // Replace with actual gamertag logic
                        Stats = new List<LeaderboardStats>()
                    };

                    foreach (var query in request.Queries)
                    {
                        var stat = new LeaderboardStats
                        {
                            Id = query.Id,
                            StatisticIds = query.StatisticIds,
                            Value = 0 // Replace with actual statistic value logic
                        };

                        leaderboardPlayer.Stats.Add(stat);
                    }

                    response.Add(new LeaderboardResponse
                    {
                        Id = 0, // Replace with actual ID logic
                        Players = new List<LeaderboardPlayer> { leaderboardPlayer }
                    });
                }

                // Return the populated response
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }


}
 public class LeaderboardsRequest
{
    public List<string> Players { get; set; }
    public string TitleId { get; set; }
    public List<Query> Queries { get; set; }
}

public class Query
{
    public long Id { get; set; }
    public List<int> StatisticIds { get; set; }
}

public class LeaderboardStats
{
    public long Id { get; set; }
    public long Type { get; set; }
    public long Value { get; set; }
    public List<int> StatisticIds { get; internal set; }
}

public class LeaderboardPlayer
{
    public string Xuid { get; set; }
    public string Gamertag { get; set; }
    public List<LeaderboardStats> Stats { get; set; }
}

public class LeaderboardResponse
{
    public long Id { get; set; }
    public List<LeaderboardPlayer> Players { get; set; }
}

public class FindLeaderboardsResponse : List<LeaderboardResponse>
{
    // This class inherits from List<LeaderboardResponse>
    // You can add any additional methods or properties if needed
}
