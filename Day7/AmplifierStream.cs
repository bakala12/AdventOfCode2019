using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Common.Streams;

namespace Day7
{
    public class AmplifierStream : IInputStream, IOutputStream
    {
        private AmplifierStream? _output;
        private readonly BlockingCollection<int> _buffer;

        public AmplifierStream(int phase)
        {
            _buffer = new BlockingCollection<int>();
            ReceiveValue(phase);
        }

        public void ConnectOutput(AmplifierStream stream)
        {
            _output = stream;
        }

        public void ReceiveValue(int value)
        {
            _buffer.Add(value);
        }

        public int Read()
        {
            int v = _buffer.Take();
            return v;
        }

        public void Write(int value)
        {
            _output?.ReceiveValue(value);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _buffer.GetConsumingEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}