namespace XeniaWebServices.XenoAPI.Request
{
    public class CreatePlayerRequest
    {
        public string Xuid { get; internal set; }
        public string MachineId { get; internal set; }
        public string HostAddress { get; internal set; }
        public string MacAddress { get; internal set; }
    }
}