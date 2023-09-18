using XeniaWebServices.Controllers;

namespace XeniaWebServices.XenoAPI.aggregates
{
    public class PlayerProps
    {
        public Xuid? Xuid { get; set; }
        public IpAddress? HostAddress { get; set; }
        public MacAddress? MacAddress { get; set; }
        public Xuid? MachineId { get; set; }
        public int Port { get; set; }
        public SessionId? SessionId { get; set; } // Use nullable SessionId for the possibility of no session
    }
}