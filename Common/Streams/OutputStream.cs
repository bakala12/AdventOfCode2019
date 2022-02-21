using System.Collections;
using System.Collections.Generic;

namespace Common.Streams
{
    public class OutputStream : IOutputStream, IEnumerable<long>
    {
        private readonly List<long> buffer = new List<long>();

        public void Write(long item)
        {
            buffer.Add(item);
        }

        public IEnumerator<long> GetEnumerator()
        {
            return buffer.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            buffer.Clear();
        }
    }
}
