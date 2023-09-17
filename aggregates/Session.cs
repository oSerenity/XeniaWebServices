using System.Security.Cryptography;
using XeniaWebServices.Controllers;
using XeniaWebServices.value_objects;

namespace XeniaWebServices.aggregates
{
    public class Session
    {
        private readonly SessionProps _props;

        public Session(SessionProps props)
        {
            _props = props;
        }

        public static Session Create(CreateProps props)
        {
            return new Session(new SessionProps
            {
                Id = props.Id,
                TitleId = props.TitleId,
                Flags = props.Flags,
                HostAddress = props.HostAddress,
                MacAddress = props.MacAddress,
                PublicSlotsCount = props.PublicSlotsCount,
                PrivateSlotsCount = props.PrivateSlotsCount,
                Port = props.Port,
                Players = new List<Xuid>(),
                Deleted = false,
            });
        }

        public static string RandomSessionId()
        {
            byte[] bytes = new byte[8];
            RandomNumberGenerator.Fill(bytes);
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static Session CreateMigration(CreateMigrationProps props)
        {
            var newSession = new Session(new SessionProps
            {
                Id = new SessionId(RandomSessionId()),
                TitleId = props.Session.TitleId,
                Flags = props.Session.Flags,
                HostAddress = props.HostAddress,
                MacAddress = props.MacAddress,
                Port = props.Port,
                PublicSlotsCount = props.Session.PublicSlotsCount,
                PrivateSlotsCount = props.Session.PrivateSlotsCount,
                Players = new List<Xuid>(),
                Deleted = false,
            });

            props.Session.Migration = newSession.Id;

            return newSession;
        }

        public void Modify(ModifyProps props)
        {
            _props.Flags = _props.Flags.Modify(props.Flags);
            _props.PrivateSlotsCount = props.PrivateSlotsCount;
            _props.PublicSlotsCount = props.PublicSlotsCount;
        }

        public void Join(JoinProps props)
        {
            _props.Players.AddRange(props.Xuids.Distinct().Select(xuid => new Xuid(xuid.Value)));
        }

        public void Leave(LeaveProps props)
        {
            _props.Players.RemoveAll(player => props.Xuids.Any(xuid => xuid.Value == player.Value));
        }

        public void Delete()
        {
            _props.Deleted = true;
        }

        public SessionId Id => _props.Id;
        public TitleId TitleId => _props.TitleId;
        public IpAddress HostAddress => _props.HostAddress;
        public SessionFlags Flags => _props.Flags;
        public int PublicSlotsCount => _props.PublicSlotsCount;
        public int PrivateSlotsCount => _props.PrivateSlotsCount;
        public int OpenPublicSlots => PublicSlotsCount - Players.Count;
        public int OpenPrivateSlots => _props.PrivateSlotsCount; // TODO: Implement this logic
        public int FilledPublicSlots => PublicSlotsCount - OpenPublicSlots;
        public int FilledPrivateSlots => PrivateSlotsCount - OpenPrivateSlots;
        public MacAddress MacAddress => _props.MacAddress;
        public int Port => _props.Port;
        public List<Xuid> Players => _props.Players;
        public bool Deleted => _props.Deleted;
        public SessionId? Migration => _props.Migration;
    }

    public class SessionProps
    {
        public SessionId Id { get; set; }
        public TitleId TitleId { get; set; }
        public SessionFlags Flags { get; set; }
        public IpAddress HostAddress { get; set; }
        public MacAddress MacAddress { get; set; }
        public int PublicSlotsCount { get; set; }
        public int PrivateSlotsCount { get; set; }
        public int Port { get; set; }
        public List<Xuid> Players { get; set; }
        public bool Deleted { get; set; }
        public SessionId? Migration { get; set; }
    }

    public class CreateProps
    {
        public SessionId Id { get; set; }
        public TitleId TitleId { get; set; }
        public SessionFlags Flags { get; set; }
        public IpAddress HostAddress { get; set; }
        public MacAddress MacAddress { get; set; }
        public int PublicSlotsCount { get; set; }
        public int PrivateSlotsCount { get; set; }
        public int Port { get; set; }
        public Xuid Xuid { get; internal set; }
        public Xuid MachineId { get; internal set; }
    }

    public class ModifyProps
    {
        public SessionFlags Flags { get; set; }
        public int PublicSlotsCount { get; set; }
        public int PrivateSlotsCount { get; set; }
    }

    public class CreateMigrationProps
    {
        public Session Session { get; set; }
        public IpAddress HostAddress { get; set; }
        public MacAddress MacAddress { get; set; }
        public int Port { get; set; }
    }

    public class JoinProps
    {
        public List<Xuid> Xuids { get; set; }
    }

    public class LeaveProps
    {
        public List<Xuid> Xuids { get; set; }
    }
}
