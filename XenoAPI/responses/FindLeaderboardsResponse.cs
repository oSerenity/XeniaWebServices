using XeniaWebServices.XenoAPI.aggregates;

namespace XeniaWebServices.XenoAPI.responses
{
    public interface LeaderboardResponse
    {
        int Id { get; set; }
        List<Player> Players { get; set; }
    }
    public class FindLeaderboardsResponse : List<LeaderboardResponse>
    {
    }
}
