using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day24
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(l => l.Select(c => c == '#').ToArray()).ToArray();
            var array = new bool[input.Length, input[0].Length];
            for(int i = 0; i < input.Length; i++)
                for(int j = 0; j < input[i].Length; j++)
                    array[i,j] = input[i][j];
            Part1(array);
            Part2(array);
        }

        private static void Part1(bool[,] array)
        {
            var initial = new bool[array.GetLength(0), array.GetLength(1)];
            var copy = new bool[array.GetLength(0), array.GetLength(1)];
            for(int i = 0; i < array.GetLength(0); i++)
                for(int j = 0; j < array.GetLength(1); j++)
                    initial[i,j] = copy[i,j] = array[i,j];
            var set = new HashSet<int>();
            var biodiversity = GetBiodiversity(initial);
            while(!set.Contains(biodiversity))
            {
                set.Add(biodiversity);
                for(int i = 0; i < initial.GetLength(0); i++)
                    for(int j = 0; j < initial.GetLength(1); j++)
                    {
                        int b = CountBugs(initial, i, j);
                        if(initial[i,j] && b != 1)
                            copy[i,j] = false;
                        else if(!initial[i,j] && (b == 1 || b == 2))
                            copy[i,j] = true;
                        else
                            copy[i,j] = initial[i,j];
                    }
                var tmp = initial;
                initial = copy;
                copy = tmp;
                biodiversity = GetBiodiversity(initial);
            }
            Console.WriteLine(biodiversity);
        }

        private static int CountBugs(bool[,] array, int i, int j)
        {
            int c = 0;
            if(i > 0 && array[i-1, j]) c++; 
            if(i < array.GetLength(0)-1 && array[i+1, j]) c++; 
            if(j > 0 && array[i, j-1]) c++; 
            if(j < array.GetLength(1)-1 && array[i, j+1]) c++; 
            return c;
        }

        private static int GetBiodiversity(bool[,] array)
        {
            int biodiversity = 0;
            int mask = 1;
            for(int i = 0; i < array.GetLength(0); i++)
                for(int j = 0; j < array.GetLength(1); j++)
                {
                    if(array[i,j])
                        biodiversity |= mask;
                    mask <<= 1;
                }
            return biodiversity;
        }
    
        private static void Part2(bool[,] array)
        {
            var bugTable = new BugsTable(array);
            for(int i = 0; i < 200; i++)
                bugTable.Iteration();
            Console.WriteLine(bugTable.CountAllBugs());
        }
    }
}