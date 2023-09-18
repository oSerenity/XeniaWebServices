namespace XeniaWebServices.Controllers
{
    public class MacAddress
    {
        public string Value { get; }

        public MacAddress(string value)
        {
            if (!IsHexString(value) || value.Length != 12)
            {
                throw new ArgumentException("Invalid MAC Address " + value);
            }

            Value = value.ToUpper();
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
            return (c >= '0' && c <= '9') ||
                   (c >= 'A' && c <= 'F') ||
                   (c >= 'a' && c <= 'f');
        }
    }

}