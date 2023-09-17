namespace XeniaWebServices.Controllers
{
    public class TitleId
    {
        private readonly int _value;

        public TitleId(string value)
        {
            if (!int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out _value))
            {
                throw new ArgumentException("Invalid hexadecimal value", nameof(value));
            }
        }

        public override string ToString()
        {
            return _value.ToString("X").ToUpper();
        }
    }
}