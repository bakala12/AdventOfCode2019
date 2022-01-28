using Common.Streams;

namespace Common.Operations
{
    public abstract class InputOutputOpCodeBase : ParameterModeOpCodeBase
    {
        public virtual void ExecuteWithInput(int[] data, int position, Parameter[] parameters, IInputStream inputStream) {}

        public virtual void ExecuteWithOutput(int[] data, int position, Parameter[] parameters, IOutputStream outputStream) {}

        protected override void ExecuteWithParameters(int[] data, int position, Parameter[] parameters)
        {
        }

        public override int Execute(int[] data, int position, int parameterModes, IInputStream inputStream, IOutputStream outputStream)
        {
            var parameters = GetParameters(data, position, parameterModes);
            ExecuteWithInput(data, position, parameters, inputStream);
            ExecuteWithOutput(data, position, parameters, outputStream);
            return NewPosition(data, position, parameters);
        }
    }
}