using XeniaWebServices.aggregates;
using XeniaWebServices.Controllers;

namespace XeniaWebServices.repositories
{
    public interface IPlayerRepository
    {
        Task<Player?> FindByXuidAsync(Xuid xuid);
        Task<Player?> FindByAddressAsync(IpAddress hostAddress);
        Task SaveAsync(Player player);
    }

    public static class IPlayerRepositoryExtensions
    {
        public static Task<Player?> FindByXuidAsync(this IPlayerRepository repository, string xuid)
        {
            return repository.FindByXuidAsync(new Xuid(xuid));
        }

        public static Task<Player?> FindByAddressAsync(this IPlayerRepository repository, string hostAddress)
        {
            return repository.FindByAddressAsync(new IpAddress(hostAddress));
        }
    }
}
