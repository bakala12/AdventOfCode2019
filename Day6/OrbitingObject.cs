namespace Day6
{
    public partial class Program
    {
        public class OrbitingObject
        {
            public string Name;
            public OrbitingObject? OrbitsAround;
            public List<OrbitingObject> Satelites = new List<OrbitingObject>();

            public OrbitingObject(string name)
            {
                Name = name;
            }

            public IEnumerable<OrbitingObject> AllNeighbours()
            {
                if(OrbitsAround != null)
                    yield return OrbitsAround;
                foreach(var sat in Satelites)
                    yield return sat;
            }
        }
    }
}