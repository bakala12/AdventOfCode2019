using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Common.Streams
{
    public class InputOutputStream : IInputStream, IOutputStream
    {
        private readonly BlockingCollection<long> _buffer = new BlockingCollection<long>();

        public long Read()
        {
            return _buffer.Take();
        }

        public void Write(long output)
        {
            _buffer.Add(output);
        }

        public IEnumerator<long> GetEnumerator()
        {
            return _buffer.GetConsumingEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
