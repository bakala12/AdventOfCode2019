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

        protected override void ExecuteWithParameters(long[] data, IntProgramState state, Parameter[] parameters)
        {
        }

        protected override IntProgramState NewState(long[] data, IntProgramState state, Parameter[] parameters)
        {
            var arg1 = parameters[0].Mode == 0 ? data[parameters[0].Value] : parameters[0].Value;
            var arg2 = parameters[1].Mode == 0 ? data[parameters[1].Value] : parameters[1].Value;
            if(_jumpCondition(arg1))
                return new IntProgramState((int)arg2, state.RelativeBase);
            return base.NewState(data, state, parameters);
        }
    }
}