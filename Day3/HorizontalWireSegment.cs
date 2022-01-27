namespace Day3
{
    public class HorizontalWireSegment : WireSegment
    {
        public override int Length { get; protected set; }

        public HorizontalWireSegment(int xStart, int xEnd, int y) : base(new Point(xStart, y), new Point(xEnd, y)) 
        {
            Length = Math.Abs(xEnd - xStart);
        }

        public override bool IntersectWith(WireSegment other, out Point intersectionPoint)
        {
            var xmin = Math.Min(Start.X, End.X);
            var xmax = Math.Max(Start.X, End.X);
            intersectionPoint = default;
            if(other is HorizontalWireSegment hs)
            {
                if(Start.Y == hs.Start.Y)
                {
                    int xmin1 = Math.Min(Start.X, End.X);
                    int xmax1 = Math.Max(Start.X, End.X);
                    int xmin2 = Math.Min(hs.Start.X, hs.End.X);
                    int xmax2 = Math.Max(hs.Start.X, hs.End.X);
                    int min = Math.Max(xmin1, xmax2);
                    int max = Math.Min(xmax1, xmax2);
                    if(min <= max)
                    {
                        if(min <= 0 && max >= 0) intersectionPoint = new Point(0, hs.Start.Y);
                        else if(min >= 0) intersectionPoint = new Point(min, hs.Start.Y);
                        else if(max <= 0) intersectionPoint = new Point(max, hs.Start.Y);
                        return true; 
                    }
                }
            }
            else if(other is VerticalWireSegment vs)
            {
                return IntersectVerticalWithHorizontal(vs, this, out intersectionPoint);
            }
            return false;
        }
    }
}