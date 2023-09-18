namespace XeniaWebServices.Controllers
{
    public class SessionId
    {
        public string Value { get; }

        public SessionId(string value)
        {
            if (!IsHexString(value) || value.Length != 16)
            {
                throw new ArgumentException("Invalid SessionId " + value);
            }

            Value = value.ToLower();
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