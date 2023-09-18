namespace XeniaWebServices.value_objects
{
    public class LeaderboardId
    {
        private readonly string value;

        public LeaderboardId(string value)
        {
            int intValue = int.Parse(value);
            this.value = (intValue & 0x0000ffff).ToString("X4");
        }

        public LeaderboardId(int value)
        {
            this.value = (value & 0x0000ffff).ToString("X4");
        }

        public override string ToString()
        {
            return value;
        }
    }

}
