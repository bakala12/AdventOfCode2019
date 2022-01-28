namespace Common.Operations
{
    public abstract class JumpOpCodeBase : ParameterModeOpCodeBase
    {
        private readonly Func<int, bool> _jumpCondition;

        public override int ParameterCount => 2;

        protected JumpOpCodeBase(Func<int, bool> jumpCondition)
        {
            _jumpCondition = jumpCondition;
        }

        protected override void ExecuteWithParameters(int[] data, int position, Parameter[] parameters)
        {
        }

        protected override int NewPosition(int[] data, int position, Parameter[] parameters)
        {
            var arg1 = parameters[0].Mode == 0 ? data[parameters[0].Value] : parameters[0].Value;
            var arg2 = parameters[1].Mode == 0 ? data[parameters[1].Value] : parameters[1].Value;
            if(_jumpCondition(arg1))
                return arg2;
            return base.NewPosition(data, position, parameters);
        }
    }
}