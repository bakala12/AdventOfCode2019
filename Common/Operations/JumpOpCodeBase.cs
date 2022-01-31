namespace Common.Operations
{
    public abstract class JumpOpCodeBase : ParameterModeOpCodeBase
    {
        private readonly Func<long, bool> _jumpCondition;

        public override int ParameterCount => 2;

        protected JumpOpCodeBase(Func<long, bool> jumpCondition)
        {
            _jumpCondition = jumpCondition;
        }

        protected override void ExecuteWithParameters(long[] data, IntProgramState state, long[] parameters)
        {
        }

        protected override IntProgramState NewState(long[] data, IntProgramState state, long[] parameters)
        {
            if(_jumpCondition(parameters[0]))
                return new IntProgramState((int)parameters[1], state.RelativeBase);
            return base.NewState(data, state, parameters);
        }
    }
}