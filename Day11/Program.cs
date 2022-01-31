using System;
using System.IO;
using System.Linq;
using Common;
using Common.Streams;
using System.Threading;

namespace Day11
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
            Part1(data);
            Part2(data);
        }

        private static void Part1(long[] data)
        {
            var inStream = new InputOutputStream();
            var outStream = new InputOutputStream();
            var program = new IntCodeProgram(inStream, outStream);
            var programTask = Task.Run(() => program.Run(data, capacity: 10000000)).ContinueWith(t => outStream.Write(999));
            var robotTask = Task.Run(() => RunRobot(inStream, outStream, false));
            var tasks = new Task[] { robotTask, programTask };
            Task.WaitAll(tasks);
            Console.WriteLine(robotTask.Result.Item1.Count);
        }

        private static void Part2(long[] data)
        {
            var inStream = new InputOutputStream();
            var outStream = new InputOutputStream();
            var program = new IntCodeProgram(inStream, outStream);
            var programTask = Task.Run(() => program.Run(data, capacity: 10000000)).ContinueWith(t => outStream.Write(999));
            var robotTask = Task.Run(() => RunRobot(inStream, outStream, true));
            var tasks = new Task[] { robotTask, programTask };
            Task.WaitAll(tasks);
            var (all, white) = robotTask.Result;
            int minX = all.Min(s => s.Item1);
            int maxX = all.Max(s => s.Item1);
            int minY = all.Min(s => s.Item2);
            int maxY = all.Max(s => s.Item2);
            for(int y = minY; y <= maxY; y++)
            {
                for(int x = minX; x <= maxX; x++)
                {
                    Console.Write(white.Contains((x,y)) ? "X" : ".");
                }
                Console.WriteLine();
            }
        }

        private static (List<(int,int)>,List<(int,int)>) RunRobot(InputOutputStream programInStream, InputOutputStream programOutStream, bool firstWhite)
        {  
            var whiteLocations = new List<(int,int)>();
            var allLocations = new List<(int,int)>();
            var currentLocation = (0,0);
            var direction = Direction.Up;
            if(firstWhite)
                whiteLocations.Add(currentLocation);
            while(true)
            {
                var inputColor = whiteLocations.Contains(currentLocation) ? 1 : 0;
                programInStream.Write(inputColor);
                var outputColor = programOutStream.Read();
                if(outputColor == 999)
                    break;
                if(outputColor == 1 && !whiteLocations.Contains(currentLocation))
                    whiteLocations.Add(currentLocation);
                if(outputColor == 0)
                    whiteLocations.Remove(currentLocation); 
                allLocations.Add(currentLocation);
                var turn = programOutStream.Read();
                (currentLocation, direction) = MoveRobotAndTurn(currentLocation, direction, turn == 0);
            }
            return (allLocations.Distinct().ToList(), whiteLocations);
        }

        private static ((int,int), Direction) MoveRobotAndTurn((int, int) location, Direction direction, bool turnLeft)
        {
            var (x,y) = location;
            direction = (Direction)(((int)direction + (turnLeft ? 1 : -1) + 4) % 4);
            switch (direction)
            {
                case Direction.Up:
                    location = (x, y-1);
                    break;
                case Direction.Down:
                    location = (x, y+1);
                    break;
                case Direction.Left:
                    location = (x-1, y);
                    break;
                case Direction.Right:
                    location = (x+1, y);
                    break;
            }
            return (location, direction);
        }

        private enum Direction
        {
            Up = 0, Left = 1, Down = 2, Right = 3
        }
    }
}