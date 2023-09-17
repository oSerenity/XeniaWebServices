using XeniaWebServices.aggregates;

namespace XeniaWebServices.responses
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
