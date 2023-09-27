using XeniaWebServices.Networking.Sessions;

namespace XeniaWebServices.Controllers
{
    internal class CreateSession : Sessions
    {
        private int titleId;

        public CreateSession(int titleId, string? sessionId, string? hostAddress, int? flags, int? publicSlotsCount, int? privateSlotsCount, string? macAddress, int? port)
        {
            this.titleId = titleId;
            SessionId = sessionId;
            HostAddress = hostAddress;
            Flags = flags;
            PublicSlotsCount = publicSlotsCount;
            PrivateSlotsCount = privateSlotsCount;
            MacAddress = macAddress;
            Port = port;
        }
    }
}