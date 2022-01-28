using System;
using System.IO;
using System.Collections.Generic;

namespace Day6
{
    public partial class Program
    {
        public static void Main()
        {
            var orbits = File.ReadAllLines("input.txt").Select(s => s.Split(')')).Select(s => (s[0], s[1])).ToArray();
            var orbitMap = MakeOrbitalMap(orbits);
            Part1(orbitMap);
            Part2(orbitMap);
        }

        private static void Part1(OrbitMap map)
        {
            if(map.Root == null)
                return;
            Console.WriteLine(FindOrbits(map.Root, 0));
        }

        private static void Part2(OrbitMap map)
        {
            Console.WriteLine(map.OrbitsBetween("YOU", "SAN") - 2);
        }

        public static int FindOrbits(OrbitingObject obj, int fromRoot)
        {       
            return fromRoot + obj.Satelites.Sum(s => FindOrbits(s, fromRoot+1));
        }

        private static OrbitMap MakeOrbitalMap((string, string)[] orbits)
        {
            var orbitMap = new OrbitMap();
            foreach(var (center, orbiting) in orbits)
                orbitMap.AddOrbit(center, orbiting);
            orbitMap.SelectRoot();
            return orbitMap;
        }
    }
}