using System.Collections;
using System.Collections.Generic;

namespace Common.Streams
{
    public class OutputStream : IOutputStream, IEnumerable<int>
    {
        private readonly List<int> buffer = new List<int>();

        public void Write(int item)
        {
            buffer.Add(item);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return buffer.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
