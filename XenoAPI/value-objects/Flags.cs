namespace XeniaWebServices.value_objects
{
    public class Flags
    {
        private readonly int _value;

        public Flags(int value)
        {
            _value = value;
        }

        private bool IsFlagSet(int flag)
        {
            return (_value & (1 << flag)) > 0;
        }

        public Flags Modify(Flags flags)
        {
            // TODO: Implement flag modification logic if needed
            // For now, we'll return the existing flags
            return new Flags(_value);
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
