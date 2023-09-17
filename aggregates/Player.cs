using System.Net.Mail;
using System;
using XeniaWebServices.Controllers;

namespace XeniaWebServices.aggregates
{
    public class Player
    {
        public string Xuid { get; private set; }
        public string HostAddress { get; private set; }
        public string MacAddress { get; private set; }
        public string MachineId { get; private set; }
        public int Port { get; private set; }
        public SessionId SessionId { get; private set; }

        public Player(string xuid, string hostAddress, string macAddress, string machineId, int port)
        {
            if (!IsHexString(xuid) || xuid.Length != 16 ||
                !IsHexString(hostAddress) || hostAddress.Length != 16 ||
                !IsHexString(macAddress) || macAddress.Length != 12 ||
                !IsHexString(machineId) || machineId.Length != 16)
            {
                throw new ArgumentException("Invalid parameter value");
            }

            Xuid = xuid;
            HostAddress = hostAddress;
            MacAddress = macAddress;
            MachineId = machineId;
            Port = port;
        }
        private bool IsHexString(string input)
        {
            foreach (char c in input)
            {
                if (!IsHexDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsHexDigit(char c)
        {
            return c >= '0' && c <= '9' ||
                   c >= 'A' && c <= 'F' ||
                   c >= 'a' && c <= 'f';
        }

        public static Player Create(string xuid, string hostAddress, string macAddress, string machineId)
        {
            return new Player(xuid, hostAddress, macAddress, machineId, 36000);
        }

        public void SetSession(SessionId sessionId)
        {
            SessionId = sessionId;
        }
    }
}