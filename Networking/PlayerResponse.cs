using Microsoft.AspNetCore.Mvc;
using XeniaWebServices.Networking.Sessions;

namespace XeniaWebServices.Networking
{
    internal class PlayerResponse
    {
        public string xuid { get; set; }
        public string hostAddress { get; set; }
        public string machineId { get; set; }
        public int port { get; set; }
        public string macAddress { get; set; }
        public string sessionId { get; set; }

    }
}