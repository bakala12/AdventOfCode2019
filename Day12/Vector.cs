namespace Day12
{
    public record struct Vector(int X, int Y, int Z)
    {
        public static Vector operator+(Vector v1, Vector v2) => new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

        public static Vector operator-(Vector v1, Vector v2) => new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

        public Vector Sign() => new Vector(Math.Sign(X), Math.Sign(Y), Math.Sign(Z));

        public int Energy => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }
}