namespace XeniaWebServices.value_objects
{
    using System;

    public class Uuid
    {
        private readonly string _value;

        private Uuid(string value)
        {
            _value = value;
        }

        public static bool IsValidValue(string value)
        {
            Guid guid;
            return Guid.TryParse(value, out guid);
        }

        public static Uuid Create(string value = "")
        {
            if (string.IsNullOrEmpty(value))
            {
                value = Guid.NewGuid().ToString();
            }

            if (!IsValidValue(value))
            {
                throw new ArgumentException($"Invalid UUID {value}.");
            }

            return new Uuid(value);
        }

        public override string ToString()
        {
            return _value;
        }
    }

}
