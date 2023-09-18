namespace XeniaWebServices.Controllers
{
    public class Xuid
    {
        public string Value { get; set; }

        public Xuid(string value)
        {
            if (!IsHexString(value) || value.Length != 16)
            {
                throw new ArgumentException("Invalid Xuid");
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