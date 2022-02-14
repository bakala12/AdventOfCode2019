using System.IO;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Streams;

namespace Day21
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
            Part1(data);
            Part2(data);
        }

        private static void Part1(long[] data)
        {
            string[] program = new string[]
            {
                "NOT A J",
                "NOT B T",
                "OR T J",
                "NOT C T",
                "OR T J",
                "AND D J",
                "WALK"
            };
            ExecuteIntCode(data, program);
        }

        private static void Part2(long[] data)
        {
            string[] program = new string[]
            {
                "NOT A J",
                "NOT B T",
                "OR T J",
                "NOT C T",
                "OR T J",
                "NOT D T",
                "AND D T",
                "OR E T",
                "OR H T",
                "AND D T",
                "AND T J",
                "RUN"  
            }; //max 15 instructions
            ExecuteIntCode(data, program);
        }

        private static void ExecuteIntCode(long[] data, string[] program)
        {
            var input = new List<long>();
            foreach(string instr in program)
            {
                foreach(char c in instr)
                    input.Add((long)c);
                input.Add((long)'\n');
            }
            var com = new IntCodeProgram(new InputStream(input), new OutputStream());
            com.Run(data, capacity: 1000000);
            foreach(var cc in com.GetOutput())
                if(cc > 256)
                    Console.WriteLine(cc);
                else
                    Console.Write((char)cc);
        }
    }
}