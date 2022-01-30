namespace Common.Operations
{
    public sealed class ExitOpCode : ParameterOnlyOpCode
    {
        public override int Code => 99;
        
        protected override IntProgramState Execute(long[] data, IntProgramState state, int parameterModes) => new IntProgramState(int.MaxValue, state.RelativeBase);
    }
}