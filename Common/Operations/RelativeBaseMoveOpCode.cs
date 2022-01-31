namespace Common.Operations
{
    public class RelativeBaseMoveOpCode : ParameterModeOpCodeBase
    {
        public override int Code => 9;

        public override int ParameterCount => 1;

        protected override void ExecuteWithParameters(long[] data, IntProgramState state, long[] parameters)
        {
        }

        protected override IntProgramState NewState(long[] data, IntProgramState state, long[] parameters)
        {
            return new IntProgramState(state.Position+2, (int)(state.RelativeBase + parameters[0]));
        }
    }
}