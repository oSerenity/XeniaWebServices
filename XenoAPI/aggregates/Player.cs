using XeniaWebServices.Controllers;

namespace XeniaWebServices.XenoAPI.aggregates
{
    public class Player
    {
        public Player(PlayerProps props)
        {
            Value = props;
        }
        public Player(string xuid)
        {
            Value.Xuid.Value = xuid;
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
            Value.SessionId = sessionId;
        }

        public Xuid Xuid
        {
            get
            {
                return Value.Xuid;
            }
            set
            {
                Value.Xuid = value;
            }
        }
        public IpAddress HostAddress => Value.HostAddress;
        public Xuid MachineId => Value.MachineId;
        public MacAddress MacAddress => Value.MacAddress;
        public int Port => Value.Port;
        public SessionId? SessionId => Value.SessionId; // Use nullable SessionId for the possibility of no session

        public PlayerProps Value { get; internal set; }
    }



}