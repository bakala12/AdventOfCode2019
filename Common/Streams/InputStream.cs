namespace Common.Streams
{
    public class InputStream : IInputStream
    {
        private readonly List<long> input;
        private int pos = 0;
        
        public InputStream(IEnumerable<long> input)
        {
            this.input = input.ToList();
        }

        public long Read()
        {
            return input[pos++];
        }
    } 
}
