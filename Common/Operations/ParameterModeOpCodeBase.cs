namespace Common.Operations
{
    public abstract class ParameterModeOpCodeBase : OpCode
    {
        public abstract int ParameterCount { get; }

        public override int Execute(int[] data, int position, int parameterModes)
        {
            var parameters = new Parameter[ParameterCount];
            for(int p = 0; p < ParameterCount; p++)
            {
                parameters[p] = new Parameter(data[position+p+1], parameterModes % 10);
                parameterModes /= 10;
            }
            ExecuteWithParameters(data, position, parameters);
            return NewPosition(data, position, parameters);
        }

        protected abstract void ExecuteWithParameters(int[] data, int position, Parameter[] parameters);

        public virtual int NewPosition(int[] data, int position, Parameter[] parameters) => position + ParameterCount + 1;

        public readonly record struct Parameter(int Value, int Mode);
    }
}