namespace XeniaWebServices.value_objects
{
    public class PropertyId
    {
        private readonly int _value;

        public PropertyId(string value)
        {
            if (!int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out _value))
            {
                throw new ArgumentException("Invalid hexadecimal value", nameof(value));
            }
        }

        public override string ToString()
        {
            return $"0x{_value.ToString("x")}";
        }
    }
}
