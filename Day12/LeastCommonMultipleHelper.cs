namespace Day12
{
    public static class LeastCommonMultipleHelper
    {
        private static long Gcd(long a, long b)
        {
            if(b > a)
            {
                long t = a;
                a = b;
                b = t;
            }
            while(b > 0)
            {
                long c = a % b;
                a = b;
                b = c;
            }
            return a;
        }

        public static long Lcm(params long[] nums)
        {
            if(nums.Length == 0)
                return 0;
            if(nums.Length == 1)
                return nums[0];
            if(nums.Length == 2)
                return nums[0] / Gcd(nums[0], nums[1]) * nums[1];
            var nums2 = new long[nums.Length-1];
            Array.Copy(nums, nums2, nums2.Length);
            return Lcm(nums[nums.Length-1], Lcm(nums2));
        }
    }
}