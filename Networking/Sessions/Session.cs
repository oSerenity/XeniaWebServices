//Handles The Logic Behind the requests

using System.Security.Cryptography;
using System.Text;

namespace XeniaWebServices.Networking.Sessions
{
    public class Session
    {

        /// <summary>
        /// Stores Sessions That Are Currently Active
        /// </summary>
        public static Dictionary<string, Session> Sessions { get; private set; } = new Dictionary<string, Session>();
        public static string Xuid { get; set; }
        public string _Xuid { get; set; }
        public static string MachineId { get; set; } 
        public string? SessionId { get; set; }
        public int Flags { get; set; }
        public int? PublicSlotsCount { get; set; }
        public int? PrivateSlotsCount { get; set; }
        public int? UserIndex { get; set; }
        public string? HostAddress { get; set; }
        public string? MacAddress { get; set; }
        public int? Port { get; set; }
        public static int StaticTitleId { get; set; } // Rename the static property
        public static string TitleId { get; set; }

        public int TitleID
        {
            get
            {
                return StaticTitleId; // Access the static property
            }
            set
            {
                StaticTitleId = value; // Modify the static property
            }
        }
        public int SearchIndex { get; internal set; }
        public int ResultsCount { get; internal set; }
        public int TotalPlayers { get; set; }
        public List<MachineDetails> Machines { get; set; }

        public class MachineDetails
        {
            public string Id { get; set; }
            public List<PlayerDetails> Players { get; set; }
        }

        public class PlayerDetails
        {
            public string Xuid { get; set; }
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
        internal static void DeleteSession(int titleId, string sessionId)
        {
            // Create the compound key using the same format as in CreateSession
            var compoundKey = $"{titleId}-{sessionId}";

            // Check if the session exists in the dictionary
            if (Sessions.ContainsKey(compoundKey))
            {
                // Remove the session from the dictionary
                Sessions.Remove(compoundKey);
            }
        }
        internal static Session CreateSession(int titleId, string sessionId, string hostAddress, int flags, int? publicSlotsCount, int? privateSlotsCount, string macAddress, int? port)
        {
            // Create a new Session object with the provided parameters
            var session = new Session
            {
                TitleID = titleId,
                SessionId = sessionId,
                HostAddress = hostAddress,
                Flags = flags,
                PublicSlotsCount = publicSlotsCount,
                PrivateSlotsCount = privateSlotsCount,
                MacAddress = macAddress,
                Port = port
            };

            // Create a compound key by concatenating titleId and sessionId
            var compoundKey = $"{titleId}-{sessionId}";

            // Add the session to the dictionary using the compound key as the key
            Sessions[compoundKey] = session;

            return session;
        }


        public bool IsHost(int flags)
        {
            return (flags & (1 << 0)) > 0;
        }

        internal static Session? Get(int titleId, string? sessionId)
        {
            if (sessionId == null)
            {
                // If sessionId is null, return null
                return null;
            }

            // Create the compound key using the same format as in CreateSession
            var compoundKey = $"{titleId}-{sessionId}";

            // Check if the session exists in the dictionary and return it if found
            if (Sessions.ContainsKey(compoundKey))
            {
                return Sessions[compoundKey];
            }

            // If the session does not exist, return null
            return null;
        }

        internal static Session? Modify(int titleId, string sessionId, int flags, int? publicSlotsCount, int? privateSlotsCount)
        {
            var compoundKey = $"{titleId}-{sessionId}";

            if (Sessions.ContainsKey(compoundKey))
            {
                var existingSession = Sessions[compoundKey];
                existingSession.SessionId = sessionId;
                existingSession.Flags = flags;
                existingSession.PublicSlotsCount = publicSlotsCount;
                existingSession.PrivateSlotsCount = privateSlotsCount;

                // Find all players associated with this session and set their sessionId
                foreach (var player in Player.ListOfPlayers.Values)
                {
                    if (player.SessionId == sessionId)
                    {
                        // Skip setting the session ID for players who already have the same session ID
                        continue;
                    }

                    player.SessionId = sessionId;
                    Console.WriteLine(player.SessionId, "-", player.Xuid);
                }

                return existingSession;
            }

            return null;
        }


        internal static void Join(int titleId, string sessionId, string xuid)
        {
            throw new NotImplementedException();
        }

        internal static void Leave(int titleId, string sessionId, string xuid)
        {
            throw new NotImplementedException();
        }

        internal static List<Session> Search(int titleId, int searchIndex, int resultsCount)
        {
            // Filter sessions by titleId
            var filteredSessions = Sessions.Values.Where(session => session.TitleID == titleId);

            // Perform the search based on searchIndex (you can implement your search logic here)
            // For simplicity, let's assume we're filtering sessions with a minimum flag value of searchIndex
            var searchedSessions = filteredSessions.Where(session => session.Flags >= searchIndex).ToList();

            // Return a limited number of results based on resultsCount
            return searchedSessions.Take(resultsCount).ToList();
        }
    }
}
