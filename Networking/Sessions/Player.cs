using System;
using System.Reflection.PortableExecutable;

namespace XeniaWebServices.Networking.Sessions
{
    public class Player
    {
        public static List<Player>? ListOfPlayers { get; private set; }
        public string? Xuid { get; set; }
        public string? MachineId { get; set; }
        public string? HostAddress { get; set; }
        public string? MacAddress { get; set; }
        public string? SessionId { get; internal set; }
        public int Port { get; internal set; } = 1001;

        /// <summary>
        /// Adds Active Player in Game to a List Of Players Currently in The Game.
        /// </summary>
        /// <param name="xuid">Players User ID</param>
        /// <param name="machineId">Players Machine ID</param>
        /// <param name="hostAddress">Players IP Address</param>
        /// <param name="macAddress">Players MacAddress</param>
        public Player(string xuid, string machineId, string hostAddress, string macAddress)
        {
            if (ListOfPlayers == null)
            {
                ListOfPlayers = new List<Player>();
            }
            ListOfPlayers.Add(new Player(xuid, machineId, hostAddress, macAddress));
        }
        /// <summary>
        /// Finds Said Player In the List Of Players Playing The Game.
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="_Player"></param>
        /// <returns></returns>
        public static Player FindPlayer(string Type, Players _Player)
        {
            switch(_Player)
            {
                case Players.MachineId:
                    return ListOfPlayers?.FirstOrDefault(player => player.MachineId == Type);
                case Players.HostAddress:
                    return ListOfPlayers?.FirstOrDefault(player => player.HostAddress == Type);
                case Players.MacAddress:
                    return ListOfPlayers?.FirstOrDefault(player => player.MacAddress == Type);
                case Players.Xuid:
                    return ListOfPlayers?.FirstOrDefault(player => player.Xuid == Type);
                default:
                    Console.WriteLine("Invalid Search Param");
                    break;

            }
            return ListOfPlayers?.FirstOrDefault(player => player.HostAddress == Type);
        }

        internal static void SetPlayerSessionId(string? xuid, string sessionId)
        {
            throw new NotImplementedException();
        }

        public enum Players
        {
            Unknown,
            Xuid,
            MachineId,
            MacAddress,
            HostAddress
        }
    }

}