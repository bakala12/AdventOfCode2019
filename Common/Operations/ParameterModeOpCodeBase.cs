namespace Common.Operations
{
    public abstract class ParameterModeOpCodeBase : ParameterOnlyOpCode
    {
        public abstract int ParameterCount { get; }

        protected override IntProgramState Execute(long[] data, IntProgramState state, int parameterModes)
        {
            var parameters = GetParameters(data, state, parameterModes);
            ExecuteWithParameters(data, state, parameters);
            return NewState(data, state, parameters);
        }

        protected abstract void ExecuteWithParameters(long[] data, IntProgramState state, long[] parameters);

        protected virtual IntProgramState NewState(long[] data, IntProgramState state, long[] parameters) => new IntProgramState(state.Position + ParameterCount + 1, state.RelativeBase);

        protected readonly record struct Parameter(long Value, int Mode);

        protected virtual long[] GetParameters(long[] data, IntProgramState state, int parameterModes)
        {
            var parameters = new long[ParameterCount];
            for(int p = 0; p < ParameterCount; p++)
            {
                parameters[p] = GetArgument(data, state, p, new Parameter(data[state.Position+p+1], parameterModes % 10));
                parameterModes /= 10;
            }
            return parameters;
        }
    
        protected virtual long GetArgument(long[] data, IntProgramState state, int parameterPosition, Parameter p)
        {
            return p.Mode switch
            {
                0 => data[p.Value],
                1 => p.Value,
                2 => data[state.RelativeBase + p.Value],
                _ => throw new InvalidOperationException("Invalid parameter mode")
            };
        }
    }
}