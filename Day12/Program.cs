using System;
using System.IO;
using System.Linq;

namespace Day12
{
    public class Program
    {
        static void Main(string[] args)
        {
            var moons = File.ReadAllLines("input.txt").Select(l => VectorParser.Parse(l)).Select(v => new Moon(v, new Vector())).ToArray();
            var copy = new Moon[moons.Length];
            Array.Copy(moons, copy, moons.Length);
            Part1(moons);
            Part2(copy);
        }

        private static void Part1(Moon[] moons)
        {
            int s = 0;
            do
            {
                MoonMover.ApplyGravityAndMove(moons);
                s++;
            } while(s < 1000);
            Console.WriteLine(moons.Sum(m => m.Energy));
        }

        private static void Part2(Moon[] moons)
        {
            int[] xStart = moons.Select(m => m.Position.X).ToArray();
            int[] yStart = moons.Select(m => m.Position.Y).ToArray();
            int[] zStart = moons.Select(m => m.Position.Z).ToArray();
            int s = 0;
            int xRepeat = -1, yRepeat = -1, zRepeat = -1;
            while(xRepeat < 0 || yRepeat < 0 || zRepeat < 0)
            {
                MoonMover.ApplyGravityAndMove(moons);
                s++;
                bool xr = true, yr = true, zr = true;
                for(int i = 0; i < moons.Length; i++)
                {
                    if(moons[i].Position.X != xStart[i])
                        xr = false;
                    if(moons[i].Position.Y != yStart[i])
                        yr = false;
                    if(moons[i].Position.Z != zStart[i])
                        zr = false;
                    if(!(xr || yr || zr)) break;
                }
                if(xr && xRepeat < 0) xRepeat = s+1;
                if(yr && yRepeat < 0) yRepeat = s+1;
                if(zr && zRepeat < 0) zRepeat = s+1;
            }
            Console.WriteLine(LeastCommonMultipleHelper.Lcm(xRepeat, yRepeat, zRepeat));
        }
    }
}