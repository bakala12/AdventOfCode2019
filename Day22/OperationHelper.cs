namespace Day22
{
    public static class OperationHelper
    {
        public static (long,long) Fold((long, long) f1, (long, long) f2, long N)
        {
            var (a1,b1) = f1;
            var (a2,b2) = f2;
            return (ProductModulo(a2,a1,N), Modulo(ProductModulo(a2, b1, N) + b2, N));      
        }

        public static long Modulo(long x, long N)
        {
            return x >= 0 ? x % N : (N-(-x % N)) % N;
        }

        public static long ProductModulo(long a, long b, long N)
        {
            while(a < 0) a += N;
            while(b < 0) b += N;
            long res = 0;
            a = a % N;
            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    res = (res + a) % N;
                } 
                a = (a * 2) % N; 
                b /= 2;
            } 
            return res % N;
        }
    }
}