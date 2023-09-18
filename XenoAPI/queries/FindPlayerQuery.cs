using XeniaWebServices.Controllers;
using XeniaWebServices.XenoAPI.commands;

namespace XeniaWebServices.XenoAPI.queries
{
    internal class FindPlayerQuery
    {
        private IpAddress ipAddress;

        public FindPlayerQuery(IpAddress ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        public string MachineId { get; internal set; }
        public Xuid Xuid { get; internal set; }
        public SessionId SessionId { get; internal set; }
        public MacAddress MacAddress { get; internal set; }
        public int Port { get; internal set; }
        public HostAddress HostAddress { get; internal set; }
    }
}