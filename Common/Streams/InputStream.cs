namespace Common.Streams
{
    public class InputStream : IInputStream
    {
        private readonly List<int> input;
        private int pos = 0;
        
        public InputStream(IEnumerable<int> input)
        {
            this.input = input.ToList();
        }

        public int Read()
        {
            return input[pos++];
        }
    } 
}
