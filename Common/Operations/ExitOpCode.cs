namespace Common.Operations
{
    public sealed class ExitOpCode : OpCode
    {
        public override int Code => 99;
        
        public override int Execute(int[] data, int position, int parameterModes) => int.MaxValue;
    }
}