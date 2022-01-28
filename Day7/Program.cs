using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Common;

namespace Day7
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(int.Parse).ToArray();
            Part1(data);
        }

        private static void Part1(int[] data)
        {
            var ap = new AmplifiersPipe(data, 5); 
            Console.WriteLine(GeneratePermutations(5).Max(p => ap.Run(p)));
        }

        public static IEnumerable<int[]> GeneratePermutations(int n)
        {
            return GeneratePermutations(new List<int>(), n);
        }

        private static IEnumerable<int[]> GeneratePermutations(List<int> current, int n)
        {
            if(current.Count == n)
                yield return current.ToArray();
            else
            {
                for(int i = 0; i < n; i++)
                {
                    if(!current.Contains(i))
                    {
                        current.Add(i);
                        foreach(var p in GeneratePermutations(current, n))
                            yield return p;
                        current.Remove(i);
                    }
                }
            }
        }
    }

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
                computer.LoadInput(new int[] { phaseSettings[i], signal});
                int[] programCopy = new int[_program.Length];
                Array.Copy(_program, programCopy, _program.Length);
                computer.Run(programCopy);
                signal = computer.GetOutput().LastOrDefault();
            }
            return signal;
        }
    }
}