using Common.Streams;

namespace Common.Operations
{
    public abstract class OpCode
    {
        public abstract int Code { get; }

        public abstract IntProgramState Execute(long[] data, IntProgramState state, int parameterModes, IInputStream inputStream, IOutputStream outputStream);
    }
}