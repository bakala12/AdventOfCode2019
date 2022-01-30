using Common;

namespace Day7
{
    public class AmplifiersPipe
    {
        private readonly int[] _program;
        private readonly int _count;

        public AmplifiersPipe(int[] program, int count)
        {
            _program = program;
            _count = count;
        }

        public int Run(int[] phaseSettings)
        {
            int signal = 0;
            for(int i = 0; i < _count; i++)
            {
                var computer = new IntCodeProgram();
                computer.LoadInput(new long[] { phaseSettings[i], signal});
                int[] programCopy = new int[_program.Length];
                Array.Copy(_program, programCopy, _program.Length);
                computer.Run(programCopy);
                signal = (int)computer.GetOutput().LastOrDefault();
            }
            return signal;
        }
    }
}