using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using XeniaWebServices.Controllers;

namespace XeniaWebServices.Networking.Sessions.Manager
{
    public class SessionManager
    {
        public static string Xuid { get; set; }
        public static string MachineId { get; set; }
        public static string HostAddress { get; set; }
        public static string MacAddress { get; set; }
        public static List<Player>? Players { get; private set; }

        public SessionManager()
        {
        }

        public static void AddPlayer(string xuid, string machineId, string hostAddress, string macAddress)
        {
            if(Players == null)
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
        public static Player FindPlayerByHostAddress(string HostAddress)
        {
            if(Players != null)
            {
                return Players.FirstOrDefault(player => player.HostAddress == HostAddress);
            }
            else
            {
                return null;
            }
        }
        public Player FindPlayerByXuid(string Xuid)
        {
            return Players.FirstOrDefault(player => player.Xuid == Xuid);
        }
        public Player FindPlayerByMachineId(string MachineId)
        {
            return Players.FirstOrDefault(player => player.MachineId == MachineId);
        }
        public Player FindPlayerByMacAddress(string MachineId)
        {
            return Players.FirstOrDefault(player => player.MacAddress == MacAddress);
        }
        public readonly Dictionary<string, Session> sessions = new Dictionary<string, Session>();

        // Implement the logic to delete the session based on titleId and sessionId
        public static async Task DeleteSessionAsync(string titleId, string sessionId)
        {

        }

        // Implement the logic to retrieve a session based on titleId and sessionId
        public static async Task<Session> GetSessionAsync(string titleId, string sessionId)
        {
            // You might need to make a database query or use a service to fetch session data.
            // Return a Session object or null if not found.
            // Replace the following line with your implementation.
            return null;
        }

    }
}
