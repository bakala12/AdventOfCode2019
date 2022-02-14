using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day20
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();
            var graph = ParseGraph(input);
            Part1(graph);
            Part2(graph, input);
            // var d = new AdventOfCode.Solutions.Year2019.Day20();
            // d.Solve();
        }

        private static void Part1(Dictionary<string, List<string>> graph)
        {
            var start = graph[graph.Keys.First(x => x.StartsWith("PORTAL AA"))][0];
            var end = graph[graph.Keys.First(x => x.StartsWith("PORTAL ZZ"))][0];
            var visited = new HashSet<string>();
            var queue = new Queue<(string, int)>();
            queue.Enqueue((start, 0));
            visited.Add(start);
            while(queue.Count > 0)
            {
                var (v,s) = queue.Dequeue();
                if(v == end)
                {
                    Console.WriteLine(s);
                    return;
                }
                foreach(var n in graph[v])
                    if(n.StartsWith("PORTAL"))
                    {
                        foreach(var nn in graph[n].Where(x => x.StartsWith("PORTAL")))
                            foreach(var nnn in graph[nn].Where(x => !x.StartsWith("PORTAL")))
                                if(!visited.Contains(nnn))
                                {
                                    visited.Add(nnn);
                                    queue.Enqueue((nnn, s+1));
                                }
                    }
                    else if(!visited.Contains(n))
                    {
                        visited.Add(n);
                        queue.Enqueue((n, s+1));
                    }
            }
        }

        private static void Part2(Dictionary<string, List<string>> graph, char[][] input)
        {
            var start = graph[graph.Keys.First(x => x.StartsWith("PORTAL AA"))][0];
            var end = graph[graph.Keys.First(x => x.StartsWith("PORTAL ZZ"))][0];
            var visited = new HashSet<(string, int)>();
            var queue = new Queue<(string, int, int)>();
            queue.Enqueue((start, 0, 0));
            visited.Add((start, 0));
            while(queue.Count > 0)
            {
                var (v,depth, steps) = queue.Dequeue();
                if(v == end && depth == 0)
                {
                    Console.WriteLine(steps);
                    return;
                }
                foreach(var n in graph[v])
                {
                    if(n.StartsWith("PORTAL"))
                    {
                        if(depth > 0 && (n.StartsWith("PORTAL AA") || n.StartsWith("PORTAL ZZ")))
                            continue;                
                        foreach(var nn in graph[n].Where(x => x.StartsWith("PORTAL")))
                            foreach(var nnn in graph[nn].Where(x => !x.StartsWith("PORTAL")))
                            {    
                                var nnPos = n.Split('=')[1].Split(',').Select(int.Parse).ToArray();
                                var portalPosition = (nnPos[0], nnPos[1]);
                                var newDepth = IsInsidePortal(portalPosition, input) ? depth + 1 : depth - 1;
                                if(!visited.Contains((nnn, newDepth)) && newDepth >= 0)
                                {
                                    visited.Add((nnn, newDepth));
                                    queue.Enqueue((nnn, newDepth, steps+1));
                                }
                            }
                    }
                    else if(!visited.Contains((n, depth)))
                    {
                        visited.Add((n, depth));
                        queue.Enqueue((n, depth, steps+1));
                    }
                }
            }
            Console.WriteLine("No path found");
        }

        private static bool IsInsidePortal((int, int) portalPosition, char[][] input)
        {
            var (i,j) = portalPosition;
            return i >= 2 && j >= 2 && i < input.Length - 2 && j < input[i].Length - 2;
        }

        private static Dictionary<string, List<string>> ParseGraph(char[][] input)
        {
            var graph = new Dictionary<string,List<string>>();
            var portals = new List<string>(); 
            for(int i = 0; i < input.Length; i++)
            {
                for(int j = 0; j < input[i].Length; j++)
                {
                    if(input[i][j] == '.')
                        graph[$"{i},{j}"] = GetNeighbours(input, i, j);
                    if(char.IsLetter(input[i][j]))
                    {
                        var name = GetPortalName(input, i, j);
                        if(!graph.TryGetValue(name, out _))
                        {
                            portals.Add(name);
                            graph[name] = new List<string>(GetNeighbours(input, i, j).Where(x => x != name));
                        }
                        else
                        {
                            graph[name].AddRange(GetNeighbours(input, i, j).Where(x => x != name));
                        }
                    }
                }
            }
            foreach(var p1 in portals)
                foreach(var p2 in portals)
                    if(p1 != p2 && p1.Split('=')[0] == p2.Split('=')[0])
                    {
                        graph[p1].Add(p2);
                        graph[p2].Add(p1);
                    }
            return graph;
        }

        private static IEnumerable<(int,int)> GetNeighbourPositions(char[][] input, int i, int j)
        {
            if(i > 0) yield return (i-1, j);
            if(j > 0) yield return (i, j-1);
            if(i < input.Length - 1) yield return (i+1, j);
            if(j < input[i].Length - 1) yield return (i, j+1);
        }

        private static List<string> GetNeighbours(char[][] input, int i, int j)
        {
            var list = new List<string>();
            foreach(var (ni,nj) in GetNeighbourPositions(input, i, j))
            {
                if(input[ni][nj] == '.')
                    list.Add($"{ni},{nj}");
                else if(char.IsLetter(input[ni][nj]))
                    list.Add(GetPortalName(input, ni, nj));
            }
            return list;
        }

        private static string GetPortalName(char[][] input, int i, int j)
        {
            char c = input[i][j];
            if(i > 0 && char.IsLetter(input[i-1][j]))
                return $"PORTAL {input[i-1][j]}{c}={i-1},{j}";
            if(i < input.Length-1 && char.IsLetter(input[i+1][j]))
                return $"PORTAL {c}{input[i+1][j]}={i},{j}";
            if(j > 0 && char.IsLetter(input[i][j-1]))
                return $"PORTAL {input[i][j-1]}{c}={i},{j-1}";
            if(j < input[i].Length - 1 && char.IsLetter(input[i][j+1]))
                return $"PORTAL {c}{input[i][j+1]}={i},{j}";
            throw new InvalidOperationException($"Invalid portal at {i},{j}");
        }        
    }
}