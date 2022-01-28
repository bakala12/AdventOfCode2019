using Common.Streams;

namespace Common.Operations
{
    public sealed class InputOpCode : InputOutputOpCodeBase
    {
        public override int Code => 3;
        public override int ParameterCount => 1;

        public override void ExecuteWithInput(int[] data, int position, Parameter[] parameters, IInputStream inputStream)
        {
            data[parameters[0].Value] = inputStream.Read();
        }
    }
}