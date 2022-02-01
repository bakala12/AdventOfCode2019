namespace Day12
{
    public static class VectorParser
    {
        public static Vector Parse(string line)
        {
            var s = line.Trim('<', '>').Split(',', '=');
            return new Vector(int.Parse(s[1]), int.Parse(s[3]), int.Parse(s[5]));
        }
    }
}