using System.Collections.Generic;

namespace Common.Streams
{
    public interface IOutputStream : IEnumerable<long>
    {
        void Write(long output);
    }
}
