using Common;

namespace Day7
{
    public class Amplifier
    {
        private readonly int[] _data;
        private readonly AmplifierStream _stream;

        public Amplifier(int[] data, int phase)
        {
            _data = new int[data.Length];
            Array.Copy(data, _data, data.Length);
            _stream = new AmplifierStream(phase);
        }

        public void ConnectOutputTo(Amplifier a)
        {
            _stream.ConnectOutput(a._stream);
        }

        public Task Start()
        {
            return Task.Run(() => Run());
        }

        public void InitialSignal(int signal)
        {
            _stream.ReceiveValue(signal);
        }

        private void Run()
        {
            var computer = new IntCodeProgram(_stream, _stream);
            computer.Run(_data);
        }

        public int GetFinalSignal()
        {
            return (int)_stream.Read();
        }
    }
}