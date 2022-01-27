using System;
using System.IO;

namespace Day4
{
    public class Program
    {
        public static void Main()
        {
            var nums = File.ReadAllText("input.txt").Split('-').Select(int.Parse).ToArray();
            Part12(nums[0], nums[1]);
        }

        private static void Part12(int minNum, int maxNum)
        {
            var min = new int[6];
            var max = new int[6];
            for(int i = 5; i >= 0; i--)
            {
                min[i] = minNum % 10;
                minNum /= 10;
                max[i] = maxNum % 10;
                maxNum /= 10;
            }
            var tab = new int[6];
            Console.WriteLine(FindPasswords(min, max, tab, 0, Has2AdjacentEqual));
            tab = new int[6];
            Console.WriteLine(FindPasswords(min, max, tab, 0, HasExactly2AdjacentEqual));
        }

        private static int FindPasswords(int[] min, int[] max, int[] tab, int pos, Func<int[], bool> verification)
        {
            if(pos == tab.Length)
                return verification(tab) ? 1 : 0;
            int pass = 0;
            for(int d = 0; d <= 9; d++)
            {
                if(pos > 0 && tab[pos-1] > d)
                    continue;
                tab[pos] = d;
                if(!IsGreaterEqual(tab, min, pos, true))
                    continue;
                if(IsGreaterEqual(tab, max, pos, false))
                    break;
                pass += FindPasswords(min, max, tab, pos+1, verification);
            }
            return pass;
        }

        private static bool IsGreaterEqual(int[] num1, int[] num2, int pos, bool equalReturn)
        {
            for(int i = 0; i <= pos; i++)
            {
                if(num1[i] > num2[i])
                    return true;
                if(num1[i] < num2[i])
                    return false;
            }
            return equalReturn;
        }

        private static bool Has2AdjacentEqual(int[] num)
        {
            for(int i = 1; i < num.Length; i++)
                if(num[i] == num[i-1])
                    return true;
            return false;
        }

        private static bool HasExactly2AdjacentEqual(int[] num)
        {
            for(int i = 1; i < num.Length; i++)
                if(num[i] == num[i-1] && (i == num.Length-1 || num[i+1] != num[i]) && (i == 1 || num[i-1] != num[i-2]))
                    return true;
            return false;
        }
    }
}