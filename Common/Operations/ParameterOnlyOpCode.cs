using Common.Streams;

namespace Common.Operations
{
    public abstract class ParameterOnlyOpCode : OpCode
    {
        public override int Execute(int[] data, int position, int parameterModes, IInputStream inputStream, IOutputStream outputStream)
        {
            return Execute(data, position, parameterModes);
        }

        protected abstract int Execute(int[] data, int position, int parameterModes);
    }
}