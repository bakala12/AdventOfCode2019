namespace Day3
{
    public readonly record struct Point(int X, int Y)
    {
        public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y);

        public static Point operator-(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }
    }
}