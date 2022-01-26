using System;
using System.IO;

namespace Day1
{
    public class Program
    {
        public static void Main()
        {
            var numbers = File.ReadAllLines("input.txt").Select(int.Parse).ToArray();
            Part1(numbers);
            Part2(numbers);
        }

        private static void Part1(int[] numbers)
        {
            Console.WriteLine(numbers.Sum(n => Math.Max(n / 3 - 2, 0)));
        }

        private static void Part2(int[] numbers)
        {
            Console.WriteLine(numbers.Sum(n => CalculateFuel(n)));
        }

        private static int CalculateFuel(int mass)
        {
            if(mass <= 0) return 0;
            var fuel = Math.Max(mass / 3 - 2, 0);
            return fuel + CalculateFuel(fuel);
        }
    }
}