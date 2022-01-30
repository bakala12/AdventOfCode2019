using System;
using System.Collections.Generic;
using Common.Operations;
using Common.Streams;

namespace Common
{
    public class IntCodeProgram
    {
        private readonly OpCodes opCodes = new OpCodes();
        private IInputStream inputStream;
        private readonly IOutputStream outputStream;

        public IntCodeProgram()
        {
            inputStream = new InputStream(new long[0]);
            outputStream = new OutputStream();
        }

        public IntCodeProgram(IInputStream inputStream, IOutputStream outputStream)
        {
            this.inputStream = inputStream;
            this.outputStream = outputStream;
        }

        public void LoadInput(IEnumerable<long> input)
        {
            inputStream = new InputStream(input);
        }

        public IEnumerable<long> GetOutput()
        {
            return outputStream;
        }

        public long Run(long[] data, Func<long[], long>? valSelector = null)
        {
            var state = new IntProgramState(0,0);
            while(state.Position < data.Length)
            {
                var d = data[state.Position];
                var opCode = opCodes[(int)(d % 100)];
                if(opCode == null)
                    throw new InvalidOperationException("Unknown code procedure");
                state = opCode.Execute(data, state, (int)(d / 100), inputStream, outputStream);
            }
            if(valSelector != null)
                return valSelector(data);
            return default;
        }

        public long Run(int[] data, Func<long[], long>? valSelector = null)
        {
            var dataEx = new long[data.Length];
            Array.Copy(data, dataEx, data.Length);
            return Run(dataEx, valSelector);
        }
    }
}