using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Common;
using Common.Streams;

namespace Day15
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
            Part12(data);
        }

        private static void Part12(long[] data)
        {
            var program = new IntCodeProgram2(data, 1000000);
            var visited = new Dictionary<(int,int), (int, int)>();
            int oxygenSteps = int.MaxValue;
            (int,int) oxygenPosition = (-1,-1);
            visited.Add((0,0),(0,0));
            var list = new List<(int,int)>();
            Explore((0,0), visited, program, 0, ref oxygenSteps, ref oxygenPosition, list);
            Console.WriteLine(oxygenSteps);
            Console.WriteLine(FillOxygen(oxygenPosition, list));
        }

        private static void Explore((int,int) position, Dictionary<(int, int), (int,int)> visited, IntCodeProgram2 program, int steps,
            ref int oxygenSteps, ref (int,int) oxygenPosition, List<(int,int)> vertices)
        {
            vertices.Add(position);
            foreach(Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var newPos = Move(position, direction);
                if(!visited.TryGetValue(newPos, out var s))
                {
                    var status = program.ExecuteTilOutput((int)direction);
                    bool moved = true;
                    visited[newPos] = ((int)status, steps+1);
                    switch(status)
                    {
                        case 0: //ok
                            moved = false;
                            break;
                        case 1: //wall
                            Explore(newPos, visited, program, steps+1, ref oxygenSteps, ref oxygenPosition, vertices);
                            break;
                        case 2: //oxygen
                            if(steps + 1 < oxygenSteps)
                            {
                                oxygenSteps = steps+1;
                                oxygenPosition = newPos;
                            }
                            Explore(newPos, visited, program, steps+1, ref oxygenSteps, ref oxygenPosition, vertices);
                            break;
                    }
                    if(moved)
                        program.ExecuteTilOutput((int)OppositeDirection(direction));
                }
            }
        }

        private static int FillOxygen((int,int) oxygenPosition, List<(int,int)> places)
        {
            var queue = new Queue<((int,int), int)>();
            queue.Enqueue((oxygenPosition,0));
            var set = new HashSet<(int,int)>();
            set.Add(oxygenPosition);
            int time = 0;
            while(queue.Count > 0)
            {
                var (pos, steps) = queue.Dequeue();
                time = Math.Max(time, steps);
                foreach(Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    var np = Move(pos, dir);
                    if(places.Contains(np) && !set.Contains(np))
                    {
                        queue.Enqueue((np, steps+1));
                        set.Add(np);
                    }
                }
            }
            return time;
        }

        private enum Direction
        {
            North = 1,
            South = 2,
            West = 3,
            East = 4
        }
    
        private static (int,int) Move((int,int) position, Direction direction)
        {
            var (x,y) = position;
            return direction switch 
            {
                Direction.North => (x, y+1),
                Direction.South => (x, y-1),
                Direction.East => (x+1, y),
                Direction.West => (x-1, y),
                _ => position
            };
        }

        private static Direction OppositeDirection(Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.East => Direction.West,
                Direction.West => Direction.East,
                _ => direction
            };
        }
    }
}