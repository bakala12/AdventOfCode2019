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
            inputStream = new InputStream(new int[0]);
            outputStream = new OutputStream();
        }

        public IntCodeProgram(IInputStream inputStream, IOutputStream outputStream)
        {
            this.inputStream = inputStream;
            this.outputStream = outputStream;
        }

        public void LoadInput(IEnumerable<int> input)
        {
            inputStream = new InputStream(input);
        }

        public IEnumerable<int> GetOutput()
        {
            return outputStream;
        }

        public void Run(int[] data)
        {
            int pos = 0;
            while(pos < data.Length)
            {
                var d = data[pos];
                var opCode = opCodes[d % 100];
                if(opCode == null)
                    throw new InvalidOperationException("Unknown code procedure");
                pos = opCode.Execute(data, pos, d / 100, inputStream, outputStream);
            }
        }
    }
}
