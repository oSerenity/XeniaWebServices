using System;
using System.Collections.Generic;
using System.Linq;

namespace XeniaWebServices.Networking.Sessions
{
    public class Player
    {
        public static Dictionary<string, Player> ListOfPlayers { get; private set; } = new Dictionary<string, Player>();

        public string? Xuid { get; set; }
        public string? MachineId { get; set; }
        public string? HostAddress { get; set; }
        public string? MacAddress { get; set; }
        public string? SessionId { get; internal set; }
        public int? Port { get; internal set; } = 1001;

        public Player(string xuid, string machineId, string hostAddress, string macAddress)
        {
            Xuid = xuid;
            MachineId = machineId;
            HostAddress = hostAddress;
            MacAddress = macAddress;
        }

        public static Player FindPlayer(string type, Players searchType)
        {
            switch (searchType)
            {
                case Players.MachineId:
                    return ListOfPlayers?.FirstOrDefault(player => player.Value.MachineId == type).Value;
                case Players.HostAddress:
                    return ListOfPlayers?.FirstOrDefault(player => player.Value.HostAddress == type).Value;
                case Players.MacAddress:
                    return ListOfPlayers?.FirstOrDefault(player => player.Value.MacAddress == type).Value;
                case Players.Xuid:
                    return ListOfPlayers?.FirstOrDefault(player => player.Value.Xuid == type).Value;
                default:
                    Console.WriteLine("Invalid Search Param");
                    break;
            }
            return null;
        }
        internal void SetPlayerSessionId(string xuid, string sessionId)
        {
            if (ListOfPlayers.TryGetValue(xuid, out var player))
            {
                player.SessionId = sessionId;
            }
        }

        internal static void Add(string xuid, string machineId, string hostAddress, string macAddress)
        {
            if (!ListOfPlayers.ContainsKey(xuid))
            {
                ListOfPlayers[hostAddress] = new Player(xuid, machineId, hostAddress, macAddress);
            }
            else
            {
                Console.WriteLine($"Player with Host Address {hostAddress} already exists. Skipping addition.");
            }
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
