using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Common;
using Common.Streams;
using System.Collections;

namespace Day23
{
    public class Program
    {
        static void Main()
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
            Part1(data);
            Part2(data);
        }

        private static void Part1(long[] data)
        {
            var computers = new IntCodeProgram2[50];
            var outputs = new OutputStream[50];
            var packets = new Queue<(long, long)>[50];
            for(int i = 0; i < 50; i++)
            {
                outputs[i] = new OutputStream();
                packets[i] = new Queue<(long, long)>();
                computers[i] = new IntCodeProgram2(data, capacity: 1000000);
                computers[i].ExecutWithInputTilInput(outputs[i], i); //configuring the address
            }
            while(true)
            {
                for(int i = 0; i < 50; i++)
                {
                    if(computers[i].IsHalted)
                    {
                        Console.WriteLine("One of machines halted. Error...");
                        return;
                    }
                    if(computers[i].IsWatingForInput)
                    {
                        var inputList = new List<long>();
                        while(packets[i].Count > 0)
                        {
                            var p = packets[i].Dequeue();
                            inputList.Add(p.Item1);
                            inputList.Add(p.Item2);
                        }
                        computers[i].ExecutWithInputTilInput(outputs[i], (inputList.Count > 0 ? inputList.ToArray() : new long[] {-1}));
                    }
                    else
                        System.Console.WriteLine("Error... Computer is not listening for packets...");
                }
                for(int i = 0; i < 50; i++)
                {
                    foreach(var o in outputs[i].Chunk(3))
                    {
                        if(o[0] >= 0 && o[0] < 50)
                            packets[o[0]].Enqueue((o[1], o[2]));
                        if(o[0] == 255)
                        {
                            Console.WriteLine(o[2]);
                            return;
                        }
                    }
                }
            }
        }

        private static void Part2(long[] data)
        {
            var computers = new IntCodeProgram2[50];
            var outputs = new OutputStream[50];
            var packets = new Queue<(long, long)>[50];
            for(int i = 0; i < 50; i++)
            {
                outputs[i] = new OutputStream();
                packets[i] = new Queue<(long, long)>();
                computers[i] = new IntCodeProgram2(data, capacity: 1000000);
                computers[i].ExecutWithInputTilInput(outputs[i], i); //configuring the address
            }
            long? lastNatY = null;
            (long,long) lastNatPacket = (0,0);
            while(true)
            {
                for(int i = 0; i < 50; i++)
                {
                    if(computers[i].IsHalted)
                    {
                        Console.WriteLine("One of machines halted. Error...");
                        return;
                    }
                    if(computers[i].IsWatingForInput)
                    {
                        var inputList = new List<long>();
                        while(packets[i].Count > 0)
                        {
                            var p = packets[i].Dequeue();
                            inputList.Add(p.Item1);
                            inputList.Add(p.Item2);
                        }
                        computers[i].ExecutWithInputTilInput(outputs[i], (inputList.Count > 0 ? inputList.ToArray() : new long[] {-1}));
                    }
                    else
                        System.Console.WriteLine("Error... Computer is not listening for packets...");
                }
                for(int i = 0; i < 50; i++)
                {
                    foreach(var o in outputs[i].Chunk(3))
                    {
                        if(o[0] >= 0 && o[0] < 50)
                            packets[o[0]].Enqueue((o[1], o[2]));
                        if(o[0] == 255)
                            lastNatPacket = (o[1], o[2]);
                    }
                    outputs[i].Clear();
                }
                if(packets.All(q => q.Count == 0))
                {
                    if(lastNatY.HasValue && lastNatY.Value == lastNatPacket.Item2)
                    {
                        Console.WriteLine(lastNatY.Value);
                        return;
                    }
                    packets[0].Enqueue(lastNatPacket);
                    lastNatY = lastNatPacket.Item2;
                }
            }
        }
    }
}