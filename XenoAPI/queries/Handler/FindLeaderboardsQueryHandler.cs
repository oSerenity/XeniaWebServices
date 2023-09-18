//using XeniaWebServices.repositories;
//using XeniaWebServices.responses;

//namespace XeniaWebServices.queries.Handler
//{
//    public class FindLeaderboardsQueryHandler //: ILeaderboardRepository//<FindLeaderboardsQuery, FindLeaderboardsResponse>
//    {
//        private readonly ILeaderboardRepository _repository;

//        public FindLeaderboardsQueryHandler(ILeaderboardRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<FindLeaderboardsResponse> Handle(FindLeaderboardsQuery query)
//        {
//            var res = new FindLeaderboardsResponse();

//            foreach (var leaderboardQuery in query.Leaderboard)
//            {
//                var leaderboardResponse = new FindLeaderboardsResponse.LeaderboardResponse
//                {
//                    Id = int.Parse(leaderboardQuery.Id.Value),
//                    Players = new List<FindLeaderboardsResponse.PlayerResponse>(),
//                };

//                foreach (var player in query.Players)
//                {
//                    var leaderboard = await _repository.FindLeaderboard(query.TitleId, leaderboardQuery.Id, player);

//                    if (leaderboard == null)
//                    {
//                        continue;
//                    }

//                    var acceptedStatIds = leaderboardQuery.StatisticIds.Select(stat => stat.Value).ToList();
//                    var stats = new List<FindLeaderboardsResponse.PlayerResponse.StatResponse>();

//                    foreach (var (statId, stat) in leaderboard.Stats)
//                    {
//                        if (acceptedStatIds.Contains(statId))
//                        {
//                            stats.Add(new FindLeaderboardsResponse.PlayerResponse.StatResponse
//                            {
//                                Id = int.Parse(statId),
//                                Type = stat.Type,
//                                Value = stat.Value,
//                            });
//                        }
//                    }

//                    leaderboardResponse.Players.Add(new FindLeaderboardsResponse.PlayerResponse
//                    {
//                        Xuid = player.Value,
//                        Gamertag = "", // You can populate this with actual data
//                        Stats = stats,
//                    });
//                }

//                if (leaderboardResponse.Players.Count > 0)
//                {
//                    res.Add(leaderboardResponse);
//                }
//            }

//            return res;
//        }
//    }
//}
