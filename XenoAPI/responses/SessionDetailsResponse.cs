using XeniaWebServices.XenoAPI.aggregates;

namespace XeniaWebServices.XenoAPI.responses
{
    public class SessionDetailsResponse : ISessionDetailsResponse
    {
        public string? Id { get; set; }
        public int Flags { get; set; }
        public string? HostAddress { get; set; }
        public int Port { get; set; }
        public string? MacAddress { get; set; }
        public int PublicSlotsCount { get; set; }
        public int PrivateSlotsCount { get; set; }
        public int OpenPublicSlotsCount { get; set; }
        public int OpenPrivateSlotsCount { get; set; }
        public int FilledPublicSlotsCount { get; set; }
        public int FilledPrivateSlotsCount { get; set; }
        public List<Player>? Players { get; set; }
    }
}
