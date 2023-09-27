namespace XeniaWebServices.Networking.Sessions
{
    public class Player
    {
        public string? Xuid { get; set; }
        public string? MachineId { get; set; }
        public string? HostAddress { get; set; }
        public string? MacAddress { get; set; }
        public string? SessionId { get; internal set; }
        public int Port { get; internal set; } = 1001;

        public Player(string xuid, string machineId, string hostAddress, string macAddress)
        {
            Xuid = xuid;
            MachineId = machineId;
            HostAddress = hostAddress;
            MacAddress = macAddress;
        }

        internal static Player FindPlayer(string hostAddress)
        {
            throw new NotImplementedException();
        }
    }

}