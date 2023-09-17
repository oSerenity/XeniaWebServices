using XeniaWebServices.Controllers;

namespace XeniaWebServices.aggregates
{
    public class Player
    {
        private readonly PlayerProps _props;

        public Player(PlayerProps props)
        {
            _props = props;
        }

        public static Player Create(CreateProps props)
        {
            return new Player(new PlayerProps
            {
                Xuid = props.Xuid,
                HostAddress = props.HostAddress,
                MacAddress = props.MacAddress,
                MachineId = props.MachineId,
                Port = 36000
            });
        }

        public void SetSession(SessionId sessionId)
        {
            _props.SessionId = sessionId;
        }

        public Xuid Xuid => _props.Xuid;
        public IpAddress HostAddress => _props.HostAddress;
        public Xuid MachineId => _props.MachineId;
        public MacAddress MacAddress => _props.MacAddress;
        public int Port => _props.Port;
        public SessionId? SessionId => _props.SessionId; // Use nullable SessionId for the possibility of no session
    }

    public class PlayerProps
    {
        public Xuid Xuid { get; set; }
        public IpAddress HostAddress { get; set; }
        public MacAddress MacAddress { get; set; }
        public Xuid MachineId { get; set; }
        public int Port { get; set; }
        public SessionId? SessionId { get; set; } // Use nullable SessionId for the possibility of no session
    }


}