namespace XeniaWebServices.Controllers
{
    public class TitleId
    {
        public static int Value;

        public TitleId(string value)
        {
            if (!int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out Value))
            {
                throw new ArgumentException("Invalid hexadecimal value", nameof(value));
            }
        }

        public override string ToString()
        {
            return Value.ToString("X").ToUpper();
        }
    }
}