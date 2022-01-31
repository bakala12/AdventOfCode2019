using Common.Streams;

namespace Common.Operations
{
    public sealed class OutputOpCode : InputOutputOpCodeBase
    {
        public override int Code => 4;
        public override int ParameterCount => 1;
        protected override int WriteableParameterPosition => -1;

        public override void ExecuteWithOutput(long[] data, IntProgramState state, long[] parameters, IOutputStream outputStream)
        {
            outputStream.Write(parameters[0]);
        }
    }
}