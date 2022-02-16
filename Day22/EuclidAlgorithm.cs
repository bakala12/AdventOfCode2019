namespace Day22
{
    public static class EuclidAlgorithm
    {
        public static long GCD(long a, long b, out long x, out long y)
        {
            var s = x = 1;
            var r = y = 0;
            if(b > a)
            {
                var t = b;
                b = a;
                a = t;
                s = x = 0;
                r = y = 1;
            }
            while(b != 0)
            {
                var c = a % b;
                var q = a / b;
                a = b;
                b = c;
                var r1 = r;
                var s1 = s;
                r = x - q * r;
                s = y - q * s;
                x = r1;
                y = s1;
            }
            return a;
        }

        public static long Invert(long a, long N)
        {
            GCD(a, N, out long x, out _);
            return x;
        }
    }
}