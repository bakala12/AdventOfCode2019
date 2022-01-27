using System;
using Common.Operations;

namespace Common
{
    public class IntCodeProgram
    {
        private readonly OpCodes opCodes = new OpCodes();

        public void Run(int[] data)
        {
            int pos = 0;
            while(pos < data.Length)
            {
                var d = data[pos];
                var opCode = opCodes[d % 100];
                if(opCode == null)
                    throw new InvalidOperationException("Unknown code procedure");
                pos = opCode.Execute(data, pos, d / 100);
            }
        }    
    }
}
