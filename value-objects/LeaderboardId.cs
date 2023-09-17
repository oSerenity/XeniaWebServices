namespace XeniaWebServices.value_objects
{
    public class LeaderboardId
    {
        private readonly string _value;

        public LeaderboardId(string value)
        {
            int intValue = int.Parse(value);
            _value = (intValue & 0x0000ffff).ToString("X4");
        }

        public LeaderboardId(int value)
        {
            _value = (value & 0x0000ffff).ToString("X4");
        }

        public override string ToString()
        {
            return _value;
        }
    }

}
