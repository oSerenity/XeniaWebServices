using XeniaWebServices.Controllers;
using XeniaWebServices.value_objects;

namespace XeniaWebServices.XenoAPI.queries
{
    public class FindLeaderboardsQuery
    {
        private List<Xuid> xuids;
        private List<LeaderboardQuery> leaderboardQueries;

        public FindLeaderboardsQuery(List<Xuid> xuids, TitleId titleId, List<LeaderboardQuery> leaderboardQueries)
        {
            this.xuids = xuids;
            TitleId = titleId;
            this.leaderboardQueries = leaderboardQueries;
        }

        public List<Xuid> Players { get; }
        public TitleId TitleId { get; }
        public List<LeaderboardQueryItem> Leaderboard { get; }


    }

    public class LeaderboardQueryItem
    {
        public LeaderboardId Id { get; }
        public List<LeaderboardStatId> StatisticIds { get; }

        public LeaderboardQueryItem(LeaderboardId id, List<LeaderboardStatId> statisticIds)
        {
            Id = id;
            StatisticIds = statisticIds;
        }
    }
}