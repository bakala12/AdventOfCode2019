namespace Common.Operations
{
    public abstract class WriteParameterModeOpCodeBase : ParameterModeOpCodeBase
    {
        protected virtual int WriteableParameterPosition => ParameterCount - 1;

        protected override long GetArgument(long[] data, IntProgramState state, int parameterPosition, Parameter p)
        {
            if(parameterPosition != WriteableParameterPosition)
                return base.GetArgument(data, state, parameterPosition, p);
            else
            {
                return p.Mode switch
                {
                    0 => p.Value,
                    2 => state.RelativeBase + p.Value,
                    _ => throw new InvalidOperationException("Invalid parameter mode for writable parameter")
                };
            }
        }
    }
}