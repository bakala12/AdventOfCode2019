using System;
using System.IO;
using Common;
using Common.Streams;

namespace Day13
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
            var inStream = new InputOutputStream();
            var outStream = new InputOutputStream();
            var computer = new IntCodeProgram(inStream, outStream);
            Task programTask = Task.Run(() => computer.Run(data, capacity: 1000000)).ContinueWith(t => outStream.Write(-1));
            var gameTask = Task.Run(() => RunGame1(outStream));
            Task.WaitAll(new Task[] { programTask, gameTask });
            Console.WriteLine(gameTask.Result);
        }

        private static long RunGame1(InputOutputStream outStream)
        {
            int blocksCount = 0;
            List<(int,int,int)> gameObjects = new List<(int, int, int)>();
            while(true)
            {
                var x = outStream.Read();
                if(x < 0) break;
                var y = outStream.Read();
                var block = outStream.Read();
                if(block == 2) blocksCount++;
                gameObjects.Add(((int)x,(int)y,(int)block));
            }
            return blocksCount;
        }

        private static void Part2(long[] data)
        {
            data[0] = 2;
            var inStream = new InputOutputStream();
            var outStream = new InputOutputStream();
            var computer = new IntCodeProgram(inStream, outStream);
            var gameTask = Task.Run(() => RunGame2(inStream, outStream));
            Task programTask = Task.Run(() => computer.Run(data, capacity: 1000000)).ContinueWith(t => 
            {
                Console.WriteLine("Game over");
                GamePending = false;
            });
            Task.WaitAll(new Task[] { programTask, gameTask });
            Console.WriteLine(gameTask.Result);
        }

        private static bool GamePending = true;

        private static long RunGame2(InputOutputStream inStream, InputOutputStream outStream)
        {
            int score = 0;
            var game = new long[22,43];
            var ball = (-1,-1), pad = (-1,-1);
            while(true)
            {
                var x = outStream.Read();
                var y = outStream.Read();
                var e = outStream.Read();
                if(x < 0)
                {
                    Display(game);
                    System.Console.WriteLine($"Score {e}");
                    break;
                }
                else
                {
                    game[y,x] = e;
                    if(e == 4)
                        ball = (x,y);
                    else if(e == 3)
                        pad = (x,y);
                }
            }
            Ball = ball;
            Pad = pad;
            var outputTask = Task.Run(() => UpdateScreen(game, outStream));
            var inputTask = Task.Run(() => ControlGame(inStream));
            outputTask.Wait();
            return score;
        }

        private static void ControlGame(InputOutputStream inStream)
        {
            while(GamePending)
            {
                var r = Console.ReadLine();
                int instruction = r[0] switch 
                {
                    'a' => -1,
                    's' => 1,
                    _ => 0
                };
                Console.WriteLine($"writing instruction {instruction}");
                inStream.Write(instruction);
                Pad.Item1 += instruction;
            }
        }

        private static (long,long) Ball;
        private static (long,long) Pad;

        private static void UpdateScreen(long[,] screen, InputOutputStream outStream)
        {
            long score = 0;
            int update = 0;
            bool lastreadSuccessed = false;
            while(GamePending)
            {
                var values = ReadValues(outStream);
                Console.WriteLine($"Read {values}");
                if(values.HasValue)
                {
                    lastreadSuccessed = true;
                    var (x,y,tile) = values.Value;
                    if(x < 0)
                        score = tile;
                    else
                    {    
                        screen[y,x] = tile;
                    }
                    if(tile == 4)
                    {
                        screen[Ball.Item2, Ball.Item1] = 0;
                        Ball = (x,y);
                    }
                }
                else if(lastreadSuccessed)
                {
                    lastreadSuccessed = false;
                    Console.WriteLine($"Update {update++}");
                    Display(screen);
                    Console.WriteLine($"Score {score}");
                }
            }
        }

        private static (long,long,long)? ReadValues(InputOutputStream outputStream)
        {
            var readTask = Task.Run(() => 
            {
                long x = outputStream.Read();
                long y = outputStream.Read();
                long tile = outputStream.Read();
                return (x,y,tile);
            });
            readTask.Wait(1000);
            if(readTask.IsCompleted)
                return readTask.Result;
            return null;
        } 

        private static void Display(long[,] game)
        {
            for(int y = 0; y < game.GetLength(0); y++)
            {
                for(int x = 0; x < game.GetLength(1); x++)
                {
                    Console.Write(game[y,x] switch 
                    {
                        0 => " ",
                        1 => "#",
                        2 => "B",
                        3 => "_",
                        4 => "o",
                        _ => $"Invalid item {game[y,x]} on ({x},{y})"
                    });
                }
                Console.WriteLine();
            }
        }
    }
}