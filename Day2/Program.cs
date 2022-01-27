using System;
using System.IO;
using System.Linq;

namespace Day2
{
    public class Program
    {
        public static void Main()
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(int.Parse).ToArray(); 
            Part1(data);
            Part2(data);
        }

        private static void Part1(int[] data)
        {
            var program = IntCodeProgramBase.Produce<IntCodeProgramDay2>();
            var numbers = new int[data.Length];
            Array.Copy(data, numbers, data.Length);
            numbers[1] = 12;
            numbers[2] = 2;
            program.Run(numbers, true);
            Console.WriteLine(numbers[0]);
        }

        private static void Part2(int[] data)
        {
            var program = IntCodeProgramBase.Produce<IntCodeProgramDay2>();
            for(int noun = 0; noun <= 99; noun++)
                for(int verb = 0; verb <= 99; verb++)
                {
                    var numbers = new int[data.Length];
                    Array.Copy(data, numbers, data.Length);
                    numbers[1] = noun;
                    numbers[2] = verb;
                    program.Run(numbers, true);
                    if(numbers[0] == 19690720)
                    {
                        Console.WriteLine(100*noun + verb);
                        return;
                    }
                }
        }
    }
}