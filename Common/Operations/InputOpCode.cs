using Common.Streams;

namespace Common.Operations
{
    public sealed class InputOpCode : InputOutputOpCodeBase
    {
        public override int Code => 3;
        public override int ParameterCount => 1;

        public override void ExecuteWithInput(long[] data, IntProgramState state, long[] parameters, IInputStream inputStream)
        {
            data[parameters[0]] = inputStream.Read();
        }
    }
}