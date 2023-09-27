using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using XeniaWebServices.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeniaWebServices.Networking.Sessions
{
    public class Sessions
    {
        public static string Xuid { get; set; }
        public static string MachineId { get; set; } 
        public static List<Player>? Players { get; private set; }  
        public string? SessionId { get; set; }
        public int Flags { get; set; }
        public int? PublicSlotsCount { get; set; }
        public int? PrivateSlotsCount { get; set; }
        public int? UserIndex { get; set; }
        public string? HostAddress { get; set; }
        public string? MacAddress { get; set; }
        public int? Port { get; set; }
        public static int TitleId { get; internal set; }

        public Sessions()
        {
        }

        public static void AddPlayer(string xuid, string machineId, string hostAddress, string macAddress)
        {
            if (Players == null)
            {
                Players = new List<Player>();
            }
            Players.Add(new Player(xuid, machineId, hostAddress, macAddress));
        }

        public static string RandomSessionId()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[8];
                rng.GetBytes(bytes);

                StringBuilder sessionId = new StringBuilder(16);

                foreach (byte b in bytes)
                {
                    sessionId.Append(b.ToString("x2"));
                }

                return sessionId.ToString();
            }
        }

        public static Player FindPlayerByHostAddress(string hostAddress)
        {
            if (Players != null)
            {
                return Players.FirstOrDefault(player => player.HostAddress == hostAddress);
            }
            else
            {
                return null;
            }
        }

        public Player FindPlayerByXuid(string xuid)
        {
            return Players?.FirstOrDefault(player => player.Xuid == xuid);
        }

        public Player FindPlayerByMachineId(string machineId)
        {
            return Players?.FirstOrDefault(player => player.MachineId == machineId);
        }

        public Player FindPlayerByMacAddress(string macAddress)
        {
            return Players?.FirstOrDefault(player => player.MacAddress == macAddress);
        }

        public readonly Dictionary<string, Sessions> sessions = new Dictionary<string, Sessions>();

        public static void DeleteSession(int titleId, string sessionId)
        {
            // Implement the logic to delete the session based on titleId and sessionId
        }
         

        internal static Sessions CreateSession(int titleId, object sessionId, string hostAddress, object flags, object publicSlotsCount, object privateSlotsCount, string macAddress, object port)
        {
            throw new NotImplementedException();
        }

        internal bool IsHost(int flags)
        {
            return (flags & (1 << 0)) > 0;
        }

        internal static Sessions GetSession(int titleId, string? sessionId)
        {
            throw new NotImplementedException();
        }

        internal static void SetPlayerSessionId(string? xuid, string sessionId)
        {
            throw new NotImplementedException();
        }
         
    }
}
