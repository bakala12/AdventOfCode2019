using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Streams;

namespace Day25
{
    public class Program
    {
        public static void Main()
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
            Part1(data);
        }

        private static void Part1(long[] data)
        {
            var vertices = new Dictionary<string, Vertex>();
            DepthFirstSearch(data, new List<string>(), new HashSet<string>(), vertices);
            var commands = ShortestPathFrom(vertices, "Hull Breach", "Security Checkpoint")["Security Checkpoint"];
            var items = CollectAllItems(vertices, commands);
            var progressCmd = FindWayToPressureSecureFloor(vertices["Security Checkpoint"]); 
            foreach(var comb in FindItemCombinationsRec(items, new List<string>(), 0))
                if(TryItemCombination(data, commands, progressCmd, comb, out string password))
                {
                    Console.WriteLine(password);
                    break;
                }
        }

        private static bool TryItemCombination(long[] data, List<string> commands, string progressCmd, string[] itemsToDrop, out string password)
        {
            var newCommands = new List<string>(commands);
            foreach(var item in itemsToDrop)
                newCommands.Add($"drop {item}");
            newCommands.Add(progressCmd);
            var output = RunCommands(data, newCommands);
            password = string.Empty;
            if(output.Contains("Analysis complete!"))
            {
                var line = output.Split('\n', StringSplitOptions.RemoveEmptyEntries).Last();
                password = line.Split(' ')[11];
                return true;
            }
            return false;
        }

        private static string FindWayToPressureSecureFloor(Vertex securityCheckpoint)
        {
            foreach(var e in securityCheckpoint.Edges)
            {
                if(e.Value.Name == "Pressure-Sensitive Floor")
                    return e.Key;
            }
            return string.Empty;
        }

        private static IEnumerable<string[]> FindItemCombinationsRec(List<string> items, List<string> currentItems, int pos)
        {
            if(pos == items.Count)
                yield return currentItems.ToArray();
            else
            {
                foreach(var c in FindItemCombinationsRec(items, currentItems, pos+1))
                    yield return c;
                currentItems.Add(items[pos]);
                foreach(var c in FindItemCombinationsRec(items, currentItems, pos+1))
                    yield return c;
                currentItems.Remove(items[pos]);
            }
        }

        private static List<string> CollectAllItems(Dictionary<string, Vertex> vertices, List<string> commands)
        {
            var verticesToVisit = vertices.Where(v => v.Key != "Security Checkpoint" && v.Key != "Pressure-Sensitive Floor").Where(v => v.Value.Items.Count > 0).Select(p => p.Key).ToList();
            var paths = ShortestPathFrom(vertices, "Security Checkpoint", verticesToVisit.ToArray());
            var items = new List<string>();
            while(verticesToVisit.Count > 0)
            {
                var v = verticesToVisit[0];
                var path = paths[v];
                var start = "Security Checkpoint";
                var stack = new Stack<string>();
                foreach(var cmd in path)
                {
                    var next = vertices[start].Edges[cmd];
                    commands.Add(cmd);
                    if(verticesToVisit.Contains(next.Name))
                    {
                        foreach(var item in next.Items)
                        {
                            commands.Add($"take {item}");
                            items.Add(item);
                        }
                    }
                    stack.Push(cmd);
                    start = next.Name;
                    verticesToVisit.Remove(start);
                }
                foreach(var cmd in stack)
                    commands.Add(Opposite(cmd));
                verticesToVisit.Remove(v);
            }
            return items;
        }

        private static string Opposite(string command)
        {
            return command switch
            {
                "north" => "south",
                "south" => "north",
                "east" => "west",
                "west" => "east",
                _ => string.Empty
            };
        }

        private static Dictionary<string, List<string>> ShortestPathFrom(Dictionary<string, Vertex> vertices, string startName, params string[] endVertices)
        {
            if(endVertices == null || endVertices.Length == 0)
                return new Dictionary<string, List<string>>();
            Dictionary<string, int> distances = vertices.ToDictionary(v => v.Key, v => int.MaxValue);
            Dictionary<string, string> previous = vertices.ToDictionary(v => v.Key, v => string.Empty);
            Dictionary<string, string> previousDir = vertices.ToDictionary(v => v.Key, v => string.Empty);
            distances[startName] = 0;
            var list = vertices.Keys.ToList();
            while(list.Count > 0)
            {
                var u = list.MinBy(k => distances[k]);
                if(u == null)
                    throw new Exception("Error in Dijekstra...");
                list.Remove(u);
                foreach(var (dir, v) in vertices[u].Edges)
                {
                    if(list.Contains(v.Name) && distances[v.Name] > distances[u] + 1)
                    {
                        distances[v.Name] = distances[u] + 1;
                        previous[v.Name] = u;
                        previousDir[v.Name] = dir;
                    }
                }
            }
            var results = new Dictionary<string, List<string>>();
            foreach(var endVertexName in endVertices)
            {
                var stack = new Stack<string>();
                var e = endVertexName;
                while(e != startName)
                {
                    stack.Push(previousDir[e]);
                    e = previous[e];
                }
                var commands = new List<string>();
                foreach(var s in stack)
                    commands.Add(s);
                results[endVertexName] = commands;
            }
            return results;
        }

        private static Vertex DepthFirstSearch(long[] data, List<string> commands, HashSet<string> visited, Dictionary<string, Vertex> vertices)
        {
            var lines = RunCommands(data, commands).Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var name = lines[0].Trim('=', ' ');
            if(visited.Contains(name))
                return vertices[name];
            var vertex = new Vertex() { Name = name };
            vertices[name] = vertex;
            visited.Add(name);
            foreach(var item in GetItems(lines))
            {
                if(!BannedItems.Contains(item))
                    vertex.Items.Add(item);
            }
            foreach(var door in GetDoors(lines))
            {
                commands.Add(door);
                vertex.Edges[door] = DepthFirstSearch(data, commands, visited, vertices);
                commands.RemoveAt(commands.Count-1);
            }
            return vertex;
        }

        private static string RunCommands(long[] data, List<string> commands)
        {
            var computer = new IntCodeProgram2(data, 1000000);
            var output = new OutputStream();
            computer.ExecutWithInputTilInput(output);
            if(commands.Count > 0)
            {
                for(int i = 0; i < commands.Count; i++)
                {
                    output.Clear();
                    var list = new List<long>(commands[i].Select(c => (long)c));
                    list.Add(10);
                    computer.ExecutWithInputTilInput(output, list.ToArray());
                }
            }
            var sb = new StringBuilder();
            foreach(var o in output)
                sb.Append((char)o);
            return sb.ToString();
        }

        private static IEnumerable<string> GetDoors(string[] lines) => GetFromSection(lines, "Doors here lead:"); 

        private static IEnumerable<string> GetItems(string[] lines) => GetFromSection(lines, "Items here:");

        private static IEnumerable<string> GetFromSection(string[] lines, string sectionHeader)
        {
            int p = 2;
            while(p < lines.Length)
            {
                if(lines[p].StartsWith(sectionHeader))
                {
                    while(lines[++p].StartsWith("-"))
                        yield return lines[p].Substring(2);
                    yield break;
                }
                p++;
            }
        }

        public class Vertex
        {
            public string Name = string.Empty;
            public List<string> Items = new List<string>();
            public Dictionary<string, Vertex> Edges = new Dictionary<string, Vertex>();
        }

        private static readonly string[] BannedItems = new [] 
        {
            "infinite loop", "photons", "molten lava", "giant electromagnet", "escape pod"
        };
    }
}