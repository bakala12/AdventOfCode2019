using System.Collections.Generic;

namespace Day6
{
    public partial class Program
    {
        public class OrbitMap
        {
            public Dictionary<string, OrbitingObject> AllObjects = new Dictionary<string, OrbitingObject>();
            public OrbitingObject? Root;

            public void AddOrbit(string center, string orbiting)
            {
                OrbitingObject? centerObj, orbitingObj;
                if(!AllObjects.TryGetValue(center, out centerObj))
                    AllObjects[center] = centerObj = new OrbitingObject(center);
                if(!AllObjects.TryGetValue(orbiting, out orbitingObj))
                    AllObjects[orbiting] = orbitingObj = new OrbitingObject(orbiting);
                orbitingObj.OrbitsAround = centerObj;
                centerObj.Satelites.Add(orbitingObj);
            }

            public void SelectRoot()
            {
                var obj = AllObjects.First().Value;
                while(obj.OrbitsAround != null)
                    obj = obj.OrbitsAround;
                Root = obj;
            }

            public int OrbitsBetween(string item1, string item2)
            {
                var obj = AllObjects[item1];
                var queue = new Queue<(OrbitingObject,int)>();
                queue.Enqueue((obj, 0));
                var visited = new HashSet<string>();
                visited.Add(item1);
                while(queue.Count > 0)
                {
                    var (o, s) = queue.Dequeue();
                    if(o.Name == item2)
                        return s;
                    foreach(var n in o.AllNeighbours())
                    {
                        if(!visited.Contains(n.Name))
                        {
                            visited.Add(n.Name);
                            queue.Enqueue((n, s+1));
                        }
                    }
                }
                return -1;
            }
        }
    }
}