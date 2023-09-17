namespace XeniaWebServices.value_objects
{
    public class SessionFlags
    {
        private readonly int _value;

        public SessionFlags(int value)
        {
            _value = value;
        }

        private bool IsFlagSet(int flag)
        {
            return (_value & (1 << flag)) > 0;
        }

        public SessionFlags Modify(SessionFlags flags)
        {
            // TODO: Implement flag modification logic if needed
            // For now, we'll return the existing flags
            return new SessionFlags(_value);
        }

        public bool Advertised
        {
            // Modify this property getter according to your logic
            get { return IsFlagSet(3); }
        }

        public bool IsHost
        {
            get { return IsFlagSet(0); }
        }
    }

}
