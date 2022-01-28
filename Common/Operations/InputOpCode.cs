using Common.Streams;

namespace Common.Operations
{
    public sealed class InputOpCode : OpCode
    {
        public override int Code => 3;

        public override int Execute(int[] data, int position, int parameterModes, IInputStream inputStream, IOutputStream outputStream)
        {
            data[data[position+1]] = inputStream.Read();
            return position + 2;
        }
    }
}