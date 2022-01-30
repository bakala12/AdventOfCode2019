namespace Common.Operations
{
    public abstract class BinaryExpressionOpCode : ParameterModeOpCodeBase
    {
        private readonly int _code;
        private readonly Func<long,long,long> _expression;

        public override int Code => _code;
        public override int ParameterCount => 3;

        protected BinaryExpressionOpCode(int code, Func<long, long, long> expression)
        {
            _code = code;
            _expression = expression;
        }

        protected override void ExecuteWithParameters(long[] data, IntProgramState position, Parameter[] parameters)
        {
            var arg1 = parameters[0].Mode == 0 ? data[parameters[0].Value] : parameters[0].Value;
            var arg2 = parameters[1].Mode == 0 ? data[parameters[1].Value] : parameters[1].Value;
            data[parameters[2].Value] = _expression(arg1, arg2);
        }
    }
}