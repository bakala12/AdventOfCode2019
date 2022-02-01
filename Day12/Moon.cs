namespace Day12
{
    public record struct Moon(Vector Position, Vector Velcity)
    {
        public int Energy => Position.Energy * Velcity.Energy;
    }
}