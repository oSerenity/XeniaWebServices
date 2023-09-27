//TODO Remake The Logic there is too many Files For Basic Logic From The Original TS Server 

namespace XeniaWebServices.Networking.Sessions
{
    public class Session
    {
        private static int _TitleId;

        public static int TitleId 
        {
            get
            {
                return _TitleId;
            }
            set
            {
                _TitleId = value;
            }
        }
        public string? SessionId { get; set; }
        public int? Flags { get; set; }
        public int? PublicSlotsCount { get; set; }
        public int? PrivateSlotsCount { get; set; }
        public int? UserIndex { get; set; } 
        public string? HostAddress { get; set; }
        public string? MacAddress { get; set; }
        public int? Port { get; set; }

        internal static void Save(string titleId)
        {
            int Value;
            int.TryParse(titleId, System.Globalization.NumberStyles.HexNumber, null, out Value);
            TitleId = Value;
        }
        internal static int Load()
        {
            return TitleId;
        }
        // Other session properties..
    }
}