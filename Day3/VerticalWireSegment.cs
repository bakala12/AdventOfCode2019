namespace Day3
{
    public class VerticalWireSegment : WireSegment
    {
        public override int Length { get; protected set; }

        public VerticalWireSegment(int yStart, int yEnd, int x) : base(new Point(x, yStart), new Point(x, yEnd)) 
        {
            Length = Math.Abs(yEnd - yStart);
        }
    
        public override bool IntersectWith(WireSegment other, out Point intersectionPoint)
        {
            var xmin = Math.Min(Start.X, End.X);
            var xmax = Math.Max(Start.X, End.X);
            intersectionPoint = default;
            if(other is HorizontalWireSegment hs)
            {
                return IntersectVerticalWithHorizontal(this, hs, out intersectionPoint);
            }
            else if(other is VerticalWireSegment vs)
            {
                if(Start.X == vs.Start.X)
                {
                    int ymin1 = Math.Min(Start.Y, End.Y);
                    int ymax1 = Math.Max(Start.Y, End.Y);
                    int ymin2 = Math.Min(vs.Start.Y, vs.End.Y);
                    int ymax2 = Math.Max(vs.Start.Y, vs.End.Y);
                    int min = Math.Max(ymin1, ymax2);
                    int max = Math.Min(ymax1, ymax2);
                    if(min <= max)
                    {
                        if(min <= 0 && max >= 0) intersectionPoint = new Point(Start.X, 0);
                        else if(min >= 0) intersectionPoint = new Point(Start.X, min);
                        else if(max <= 0) intersectionPoint = new Point(Start.X, max);
                        return true; 
                    }
                }
            }
            return false;
        }
    }
}