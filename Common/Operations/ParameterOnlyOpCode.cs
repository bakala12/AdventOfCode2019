using Common.Streams;

namespace Common.Operations
{
    public abstract class ParameterOnlyOpCode : OpCode
    {
        public override IntProgramState Execute(long[] data, IntProgramState state, int parameterModes, IInputStream inputStream, IOutputStream outputStream)
        {
            return Execute(data, state, parameterModes);
        }

        protected abstract IntProgramState Execute(long[] data, IntProgramState state, int parameterModes);
    }
}