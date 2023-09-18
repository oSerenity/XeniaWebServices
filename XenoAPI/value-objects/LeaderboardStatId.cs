namespace XeniaWebServices.value_objects
{
    public class LeaderboardStatId
    {
        private readonly string _value;

        public LeaderboardStatId(string value)
        {
            _value = value;
        }

        public LeaderboardStatId(int value)
        {
            _value = value.ToString();
        }

        public override string ToString()
        {
            return _value;
        }
    }

}
