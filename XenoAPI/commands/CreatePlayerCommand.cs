using XeniaWebServices.Controllers;

namespace XeniaWebServices.XenoAPI.commands
{
    internal class CreatePlayerCommand
    {
        private Xuid xuid1;
        private Xuid xuid2;
        private IpAddress ipAddress;
        private MacAddress macAddress;

        public CreatePlayerCommand(Xuid xuid1, Xuid xuid2, IpAddress ipAddress, MacAddress macAddress)
        {
            this.xuid1 = xuid1;
            this.xuid2 = xuid2;
            this.ipAddress = ipAddress;
            this.macAddress = macAddress;
        }
    }
}