using XeniaWebServices.aggregates;
using XeniaWebServices.Controllers;

namespace XeniaWebServices.repositories
{
    public interface ISessionRepository
    {
        Task<List<Session>> FindAdvertisedSessionsAsync(TitleId titleId, int resultsCount);
        Task<Session?> FindSessionAsync(TitleId titleId, SessionId id);
        Task<Session> FindByPlayerAsync(Xuid xuid);
        Task SaveAsync(Session session);
    }

    public static class ISessionRepositoryExtensions
    {
        public static Task<Session?> FindSessionAsync(this ISessionRepository repository, string titleId, string id)
        {
            return repository.FindSessionAsync(new TitleId(titleId), new SessionId(id));
        }

        public static Task<Session> FindByPlayerAsync(this ISessionRepository repository, string xuid)
        {
            return repository.FindByPlayerAsync(new Xuid(xuid));
        }
    }
}
