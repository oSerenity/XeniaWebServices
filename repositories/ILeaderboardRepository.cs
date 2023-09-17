using XeniaWebServices.aggregates;
using XeniaWebServices.Controllers;
using XeniaWebServices.value_objects;

namespace XeniaWebServices.repositories
{
    public interface ILeaderboardRepository
    {
        Task<Leaderboard?> FindLeaderboardAsync(TitleId titleId, LeaderboardId id, Xuid player);
        Task SaveAsync(Leaderboard leaderboard);
    }

    public static class ILeaderboardRepositoryExtensions
    {
        public static Task<Leaderboard?> FindLeaderboardAsync(this ILeaderboardRepository repository, string titleId, string id, string player)
        {
            return repository.FindLeaderboardAsync(new TitleId(titleId), new LeaderboardId(id), new Xuid(player));
        }
    }
}
