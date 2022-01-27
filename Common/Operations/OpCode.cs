namespace Common.Operations
{
    public abstract class OpCode
    {
        public abstract int Code { get; }

        public abstract int Execute(int[] data, int position, int parameterModes);
    }
}