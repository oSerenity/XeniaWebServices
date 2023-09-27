namespace XeniaWebServices.Networking
{
    public class ConnectionInfo
    {
        public string info { get; set; }
        public int port { get; set; }
        public int mappedTo { get; set; }
    }

    public class Ports
    {
        public List<ConnectionInfo> connect { get; set; }
        public List<ConnectionInfo> bind { get; set; }
    }
}
