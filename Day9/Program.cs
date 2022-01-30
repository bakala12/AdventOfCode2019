using System;
using System.IO;
using System.Linq;
using Common;

namespace Day9
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
            Part1(data);
        }

        private static void Part1(long[] data)
        {

        }
    }
}