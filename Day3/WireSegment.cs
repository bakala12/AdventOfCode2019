namespace Day3
{
    public abstract class WireSegment
    {
        public Point Start { get; private set; }
        public Point End { get; private set; }

        public abstract int Length { get; protected set; }

        protected WireSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public abstract bool IntersectWith(WireSegment other, out Point intersectionPoint);

        protected bool IntersectVerticalWithHorizontal(VerticalWireSegment vs, HorizontalWireSegment hs, out Point intersectionPoint)
        {
            intersectionPoint = default;
            int xmin = Math.Min(hs.Start.X, hs.End.X);
            int xmax = Math.Max(hs.Start.X, hs.End.X);
            if(vs.Start.X >= xmin && vs.Start.X <= xmax)
            {
                int ymin = Math.Min(vs.Start.Y, vs.End.Y);
                int ymax = Math.Max(vs.Start.Y, vs.End.Y);
                if(hs.Start.Y >= ymin && hs.Start.Y <= ymax)
                {
                    intersectionPoint = new Point(vs.Start.X, hs.Start.Y);
                    return true;
                }
            }
            return false;
        }
    }
}