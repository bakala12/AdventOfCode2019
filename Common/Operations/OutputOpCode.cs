using Common.Streams;

namespace Common.Operations
{
    public sealed class OutputOpCode : OpCode
    {
        public override int Code => 4;

        public override int Execute(int[] data, int position, int parameterModes, IInputStream inputStream, IOutputStream outputStream)
        {
            outputStream.Write(data[data[position+1]]);
            return position+2;
        }
    }
}