using XeniaWebServices.value_objects;

namespace XeniaWebServices.XenoAPI.queries
{
    public class LeaderboardQuery
    {
        public LeaderboardId Id { get; set; }
        public List<LeaderboardStatId> StatisticIds { get; set; }
    }
}