using System.Collections.Generic;

namespace Common.Streams
{
    public interface IOutputStream : IEnumerable<int>
    {
        void Write(int output);
    }
}
