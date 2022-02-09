using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day16
{
    public class Program
    {
        static void Main(string[] args)
        {
            var array = File.ReadAllText("input.txt").Select(c => (byte)(c - '0')).ToArray();
            Part1(array.ToList(), new int[] { 0, 1, 0, -1}, 100);
            Part2(array, 100);
        }

        private static void Part1(List<byte> list, int[] patternBase, int iterations)
        {
            for(int i = 0; i < iterations; i++)
            {
                var newList = new List<byte>();
                for(int p = 0; p < list.Count; p++)
                {
                    var pattern = ProducePattern(patternBase, p+1, list.Count+1).Skip(1).ToArray();
                    int s = 0;
                    for(int j = 0; j < list.Count; j++)
                    {
                        s += list[j] * pattern[j];
                    }
                    newList.Add((byte)(Math.Abs(s) % 10));
                }
                list = newList;
            }
            for(int i = 0; i < 8; i++)
                Console.Write(list[i]);
            Console.WriteLine();
        }

        private static IEnumerable<int> ProducePattern(int[] patternBase, int position, int count)
        {
            int i = 0;
            int c = 0;
            while(c < count)
            {
                for(int p = 0; p < position; p++)
                {
                    yield return patternBase[i];
                    c++;
                }
                i++;
                i %= patternBase.Length;
            }
        }

        private static void Part2(byte[] input, int iterations)
        {
            var array = new byte[input.Length * 10000];
            for(int i = 0; i < 10000; i++)
                Array.Copy(input, 0, array, i*input.Length, input.Length);
            var offset = int.Parse(string.Join("", input.Take(7)));
            var results = new byte[array.Length];
            for(int it = 0; it < 100; it++)
            {
                int sum = 0;
                for(int i = offset; i < array.Length; i++)
                    sum += array[i];
                for(int i = offset; i < array.Length; i++)
                {
                    results[i] = (byte)(sum % 10);
                    sum -= array[i];
                }
                var tmp = array;
                array = results;
                results = tmp;
            }
            Console.WriteLine(string.Join("", array.Skip(offset).Take(8)));
        }
    }
}