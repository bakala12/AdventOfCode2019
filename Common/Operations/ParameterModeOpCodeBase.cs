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

        protected abstract void ExecuteWithParameters(long[] data, IntProgramState state, Parameter[] parameters);

        protected virtual IntProgramState NewState(long[] data, IntProgramState state, Parameter[] parameters) => new IntProgramState(state.Position + ParameterCount + 1, state.RelativeBase);

        public readonly record struct Parameter(long Value, int Mode);

        protected virtual Parameter[] GetParameters(long[] data, IntProgramState state, int parameterModes)
        {
            var parameters = new Parameter[ParameterCount];
            for(int p = 0; p < ParameterCount; p++)
            {
                parameters[p] = new Parameter(data[state.Position+p+1], parameterModes % 10);
                parameterModes /= 10;
            }
            return parameters;
        }
    }
}