using Common.Streams;

namespace Common.Operations
{
    public sealed class OutputOpCode : InputOutputOpCodeBase
    {
        public override int Code => 4;
        public override int ParameterCount => 1;

        public override void ExecuteWithOutput(int[] data, int position, Parameter[] parameters, IOutputStream outputStream)
        {
            var arg = parameters[0].Mode == 0 ? data[parameters[0].Value] : parameters[0].Value;
            outputStream.Write(arg);
        }
    }
}