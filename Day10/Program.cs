using System;
using System.IO;
using System.Linq;

namespace Day10
{
    public class Program
    {
        static void Main(string[] args)
        {
            var map = File.ReadAllLines("input.txt");
            var (y,x) = Part1(map);
            Part2(map, y, x);
        }

        private static (int,int) Part1(string[] map)
        {
            int height = map.Length;
            int width = map[0].Length;
            var visibilityCount = new int[height, width];
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    if(map[y][x] == '#')
                    {
                        for(int ys = height-1-y; ys >= -y; ys--)
                        {
                            for(int xs = width-1-x; xs >= -x; xs--)
                            {
                                if(GreatestCommonDivisor(xs, ys) == 1)
                                {
                                    int xm = x;
                                    int ym = y;
                                    while(true)
                                    {
                                        xm += xs;
                                        ym += ys;
                                        if(xm < 0 || xm >= width || ym < 0 || ym >= height)
                                            break;
                                        if(map[ym][xm] == '#')
                                        {
                                            visibilityCount[y,x]++;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            int max = int.MinValue;
            int yl = 0, xl = 0;
            for(int y = 0; y < height; y++)
                for(int x = 0; x < width; x++)
                    if(max < visibilityCount[y,x])
                    {
                        max = visibilityCount[y,x];
                        yl = y;
                        xl = x;
                    }
            Console.WriteLine(max);
            return (yl, xl);
        }

        private static int GreatestCommonDivisor(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            if(a < b)
            {
                int t = a;
                a = b;
                b = t;
            }
            while(b > 0)
            {
                int c = a % b;
                a = b;
                b = c;
            }
            return a;
        }
    
        private static void Part2(string[] map, int y, int x)
        {
            int height = map.Length;
            int width = map[0].Length;
            var asteroids = new bool[height, width];
            for(int i = 0; i < height; i++)
                for(int j = 0; j < width; j++)
                        asteroids[i,j] = map[i][j] == '#';
            var vaporized = new List<(int, int)>();
            while(true)
            {
                int num = 0;
                foreach(var (ya, xa, phi) in GetAsteroidsVisibleOrdered(asteroids, y, x))
                {
                    asteroids[ya, xa] = false;
                    vaporized.Add((xa, ya));
                    num++;
                }
                if(num == 0)
                    break;
            }
            Console.WriteLine(vaporized[199].Item1*100+vaporized[199].Item2);
        }

        private static IEnumerable<(int, int)> GetAsteroidsVisible(bool[,] map, int y, int x)
        {
            for(int ys = map.GetLength(0)-1-y; ys >= -y; ys--)
            {
                for(int xs = map.GetLength(1)-1-x; xs >= -x; xs--)
                {
                    if(GreatestCommonDivisor(xs, ys) == 1)
                    {
                        int xm = x;
                        int ym = y;
                        while(true)
                        {
                            xm += xs;
                            ym += ys;
                            if(xm < 0 || xm >= map.GetLength(1) || ym < 0 || ym >= map.GetLength(0))
                                break;
                            if(map[ym, xm])
                            {
                                yield return (ym, xm);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static IEnumerable<(int, int, double)> GetAsteroidsVisibleOrdered(bool[,] map, int y, int x)
        {
            return GetAsteroidsVisible(map, y, x).Select(p =>
            {
                var phi = Math.Atan2(y-p.Item1, p.Item2-x);
                if(phi > Math.PI/2)
                    phi = -2*Math.PI + phi;
                return (p.Item1, p.Item2, phi);
            }).OrderByDescending(t => t.Item3);
        }
    }
}