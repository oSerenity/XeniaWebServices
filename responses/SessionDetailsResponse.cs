using XeniaWebServices.aggregates;

namespace XeniaWebServices.responses
{
    public interface SessionDetailsResponse
    {
        string Id { get; set; }
        int Flags { get; set; }
        string HostAddress { get; set; }
        int Port { get; set; }
        string MacAddress { get; set; }
        int PublicSlotsCount { get; set; }
        int PrivateSlotsCount { get; set; }
        int OpenPublicSlotsCount { get; set; }
        int OpenPrivateSlotsCount { get; set; }
        int FilledPublicSlotsCount { get; set; }
        int FilledPrivateSlotsCount { get; set; }
        List<Player> Players { get; set; }
    }
}
