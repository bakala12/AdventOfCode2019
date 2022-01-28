using Common.Streams;

namespace Day7
{
    public class AmplifiersLoop
    {
        private readonly int[] _program;
        private readonly int _count;

        public AmplifiersLoop(int[] program, int count)
        {
            _program = program;
            _count = count;
        }

        public int Run(int[] phaseSettings)
        {
            var amplifiers = new Amplifier[phaseSettings.Length];
            for(int i = 0; i < phaseSettings.Length; i++)
                amplifiers[i] = new Amplifier(_program, phaseSettings[i]);
            
            for(int i = 1; i < amplifiers.Length; i++)
                amplifiers[i-1].ConnectOutputTo(amplifiers[i]);
            amplifiers[amplifiers.Length-1].ConnectOutputTo(amplifiers[0]);
            
            var tasks = new List<Task>();
            foreach(var a in amplifiers)
                tasks.Add(a.Start());
            amplifiers[0].InitialSignal(0);

            Task.WaitAll(tasks.ToArray());

            return amplifiers[0].GetFinalSignal();
        }
    }
}