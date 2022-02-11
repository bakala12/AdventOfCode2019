using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day18
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();
            var sw = new System.Diagnostics.Stopwatch();
            Part1(input);
            Part2(input);
        }

        private static void Part1(char[][] input)
        {
            int keysCount = 0;
            var inputPosition = (-1,-1);
            for(int i = 0; i < input.Length; i++)
                for(int j = 0; j < input[i].Length; j++)
                {
                    if(input[i][j] == '@')
                        inputPosition = (i,j);
                    else if(char.IsLetter(input[i][j]) && char.IsLower(input[i][j]))
                        keysCount++;
                }
            int allKeys = 0;
            for(int k = 0; k < keysCount; k++)
            {
                allKeys <<= 1;
                allKeys |= 1;
            }
            var cost = FindBestCost(inputPosition, input, 0, allKeys, keysCount, new Dictionary<int, int>());
            Console.WriteLine(cost);
        }

        private static IEnumerable<(int,int)> GetNeighboirs(int i, int j, char[][] input)
        {
            if(i > 0 && input[i-1][j] != '#')
                yield return (i-1,j);
            if(i < input.Length - 1 && input[i+1][j] != '#')
                yield return (i+1,j);
            if(j > 0 && input[i][j-1] != '#')
                yield return (i,j-1);
            if(j < input[i].Length - 1 && input[i][j+1] != '#')
                yield return (i,j+1);
        }

        private static IEnumerable<((int,int),int)> GetMoves((int,int) from, char[][] input, int availableKeys)
        {
            var queue = new Queue<((int,int),int)>();
            var visited = new HashSet<(int,int)>();
            queue.Enqueue((from, 0));
            visited.Add(from);
            while(queue.Count > 0)
            {
                var ((i,j), steps) = queue.Dequeue();
                var c = input[i][j];
                int keyMask = 1 << (c-'a');
                bool cont = true;
                if(char.IsLetter(c))
                {
                    if(char.IsLower(c))
                    {
                        if((availableKeys & keyMask) == 0)
                        {
                            yield return ((i,j), steps);
                            cont = false;
                        }
                    }
                    else
                        cont = (availableKeys & keyMask) != 0;
                }
                if(cont)
                {                
                    foreach(var pos in GetNeighboirs(i, j, input))
                    {
                        if(!visited.Contains(pos))
                        {
                            visited.Add(pos);
                            queue.Enqueue((pos, steps+1));
                        }
                    }
                }
            }
        }
    
        private static int FindBestCost((int,int) position, char[][] input, int availableKeys, int allKeys, int keysCount, Dictionary<int, int> distanceCache, int lastKey = 0)
        {
            if(availableKeys == allKeys)
                return 0;
            var key = availableKeys | (lastKey << keysCount);
            if(distanceCache.TryGetValue(key, out int val))
                return val;
            int best = int.MaxValue;
            foreach(var ((i,j), steps) in GetMoves(position, input, availableKeys))
            {
                lastKey = input[i][j] - 'a';
                var newKeys = availableKeys | (1 << lastKey);                
                int c = FindBestCost((i,j), input, newKeys, allKeys, keysCount, distanceCache, lastKey) + steps;
                if(c < best)
                    best = c;
            }
            distanceCache[key] = best;
            return best;
        }
    
        private static void Part2(char[][] input)
        {
            int keysCount = 0;
            var inputPosition = (-1,-1);
            for(int i = 0; i < input.Length; i++)
                for(int j = 0; j < input[i].Length; j++)
                {
                    if(input[i][j] == '@')
                        inputPosition = (i,j);
                    else if(char.IsLetter(input[i][j]) && char.IsLower(input[i][j]))
                        keysCount++;
                }
            int allKeys = 0;
            for(int k = 0; k < keysCount; k++)
            {
                allKeys <<= 1;
                allKeys |= 1;
            }
            var (i1,j1) = inputPosition;
            input[i1-1][j1-1] = input[i1-1][j1+1] = input[i1+1][j1-1] = input[i1+1][j1+1] = '@';
            input[i1-1][j1] = input[i1+1][j1] = input[i1][j1-1] = input[i1][j1+1] = input[i1][j1] = '#';
            var robots = new (int,int)[] { (i1-1, j1-1), (i1-1, j1+1), (i1+1, j1-1), (i1+1, j1+1)};
            var cost = FindBestCost2(robots, input, 0, allKeys, keysCount, new Dictionary<long, int>(), new int[robots.Length], 5);
            Console.WriteLine(cost);
        }

        private static int FindBestCost2((int,int)[] robots, char[][] input, int availableKeys, int allKeys, int keysCount, Dictionary<long, int> distanceCache, int[] robotKeys, int keysBits)
        {
            if(availableKeys == allKeys)
                return 0;
            long robotKey = 0;
            for(int k = 0; k < robotKeys.Length; k++)
            {
                robotKey <<= keysBits;
                robotKey |= (long)robotKeys[k];
            }
            long key = robotKey | ((long)availableKeys) << (keysBits * robotKeys.Length);
            if(distanceCache.TryGetValue(key, out int val))
                return val;
            int best = int.MaxValue;
            for(int k = 0; k < robots.Length; k++)
            {
                foreach(var ((i,j), steps) in GetMoves(robots[k], input, availableKeys))
                {
                    var lastKey = input[i][j] - 'a';
                    var newKeys = availableKeys | (1 << lastKey);
                    var oldPosition = robots[k];
                    robots[k] = (i,j);
                    var oldKey = robotKeys[k];
                    robotKeys[k] = lastKey;
                    int c = FindBestCost2(robots, input, newKeys, allKeys, keysCount, distanceCache, robotKeys, keysBits) + steps;
                    robots[k] = oldPosition;
                    robotKeys[k] = oldKey;
                    if(c < best)
                        best = c;
                }
            }
            distanceCache[key] = best;
            return best;
        }
    }
}