namespace Common.Operations
{
    public sealed class ExitOpCode : ParameterOnlyOpCode
    {
        public override int Code => 99;
        
        protected override int Execute(int[] data, int position, int parameterModes) => int.MaxValue;
    }
}