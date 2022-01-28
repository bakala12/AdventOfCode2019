using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day7
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(int.Parse).ToArray();
            Part1(data);
            Part2(data);
        }

        private static void Part1(int[] data)
        {
            var ap = new AmplifiersPipe(data, 5); 
            Console.WriteLine(GeneratePermutations(0, 4).Max(p => ap.Run(p)));
        }

        private static void Part2(int[] data)
        {
            Console.WriteLine(GeneratePermutations(5, 9).Max(p => new AmplifiersLoop(data, 5).Run(p)));
        }

        public static IEnumerable<int[]> GeneratePermutations(int from, int to)
        {
            return GeneratePermutations(new List<int>(), from, to);
        }

        private static IEnumerable<int[]> GeneratePermutations(List<int> current, int from, int to)
        {
            if(current.Count == to - from + 1)
                yield return current.ToArray();
            else
            {
                for(int i = from; i <= to; i++)
                {
                    if(!current.Contains(i))
                    {
                        current.Add(i);
                        foreach(var p in GeneratePermutations(current, from, to))
                            yield return p;
                        current.Remove(i);
                    }
                }
            }
        }
    }
}