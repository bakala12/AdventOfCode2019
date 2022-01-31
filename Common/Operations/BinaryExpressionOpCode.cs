namespace Common.Operations
{
    public abstract class BinaryExpressionOpCode : WriteParameterModeOpCodeBase
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

        protected override void ExecuteWithParameters(long[] data, IntProgramState state, long[] parameters)
        {
            data[parameters[2]] = _expression(parameters[0], parameters[1]);
        }
    }
}