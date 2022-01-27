using System;

namespace Day2
{
    public abstract class IntCodeProgramBase
    {
        protected readonly Func<int[], int, int>[] _opCodes = new Func<int[], int, int>[100];

        protected IntCodeProgramBase() {}

        public static TSelf Produce<TSelf>()
            where TSelf : IntCodeProgramBase, new()
        {
            var program = new TSelf();
            program.RegisterOpCodes();
            return program;
        }

        public void Run(int[] numbers, bool debug = false)
        {
            int pos = 0;
            while(pos < numbers.Length)
            {
                var codeProcedure = _opCodes[numbers[pos]];
                if(codeProcedure == null)
                    throw new InvalidOperationException("Unknown code procedure");
                pos = codeProcedure(numbers, pos);
            }
        }

        protected abstract void RegisterOpCodes();
    }
}