using Common.Operations;
using Common.Streams;

namespace Common
{
    public class IntCodeProgram2
    {
        private readonly OpCodes opCodes = new OpCodes();
        private long[] _data;
        private IntProgramState _state;

        public IntCodeProgram2(long[] data, int capacity = 0)
        {
            if(capacity < data.Length)
                capacity = data.Length;
            _data = new long[capacity];
            Array.Copy(data, _data, data.Length);
            _state = new IntProgramState(0,0);
        }

        public long ExecuteTilOutput(IInputStream inputStream)
        {
            var outputStream = new InputOutputStream();
            while(_state.Position < _data.Length)
            {
                var d = _data[_state.Position];
                var opCode = opCodes[(int)(d % 100)];
                if(opCode == null)
                    throw new InvalidOperationException("Unknown code procedure");
                _state = opCode.Execute(_data, _state, (int)(d / 100), inputStream, outputStream);
                if(opCode.Code == 4)
                    return outputStream.Read();
                if(opCode.Code == 99)
                    break;
            }
            return 99;
        }

        public long ExecuteTilOutput(params long[] input) => ExecuteTilOutput(new InputStream(input));

        public void ExecutWithInputTilInput(IOutputStream outputStream, params long[] inputs)
        {
            if(IsHalted)
                return;
            IsWatingForInput = false;
            int pos = 0;
            IInputStream inputStream = new InputStream(inputs);
            while(_state.Position < _data.Length)
            {
                var d = _data[_state.Position];
                var opCode = opCodes[(int)(d % 100)];
                if(opCode == null)
                    throw new InvalidOperationException("Unknown code procedure");
                if(opCode.Code == 3 && pos++ >= inputs.Length)
                {
                    IsWatingForInput = true;
                    return;
                }
                _state = opCode.Execute(_data, _state, (int)(d / 100), inputStream, outputStream);
                if(opCode.Code == 99)
                {
                    IsHalted = true;
                    return;
                }
            }
        }

        public bool IsHalted { get; private set; } = false;
        public bool IsWatingForInput { get; private set; } = false;
    }
}