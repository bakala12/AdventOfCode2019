using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day8
{
    public class Program
    {
        static void Main(string[] args)
        {
            var image = File.ReadAllText("input.txt").Select(c => c - '0').ToArray();
            var layers = Decode(image, 25, 6);
            Part1(layers);
            Part2(layers, 25, 6);
        }

        private static void Part1(int[][] layers)
        {
            var best = layers[0];
            int zeroes = best.Count(c => c == 0);
            for(int i = 1; i < layers.Length; i++)
            {
                int z = layers[i].Count(c => c == 0);
                if(z < zeroes)
                {
                    zeroes = z;
                    best = layers[i];
                }
            }
            Console.WriteLine(best.Count(c => c == 1) * best.Count(c => c == 2));
        }

        private static void Part2(int[][] layers, int width, int height)
        {
            var image = new int[layers[0].Length];
            Array.Copy(layers[0], image, image.Length);
            for(int i = 1; i < layers.Length; i++)
            {
                for(int j = 0; j < image.Length; j++)
                    if(image[j] == 2)
                        image[j] = layers[i][j];
            }
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                    Console.Write(image[i*width+j] == 1 ? '#' : '.');
                Console.WriteLine();
            }
        }

        private static int[][] Decode(int[] image, int width, int height)
        {
            int layerSize = width * height;
            var layers = new int[image.Length/layerSize][];
            for(int i = 0; i < layers.Length; i++)
            {
                layers[i] = new int[layerSize];
                Array.Copy(image, layerSize*i, layers[i], 0, layerSize);
            }
            return layers;
        }
    }
}