using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day22
{
    public class Program
    {
        static void Main(string[] args)
        {
            var instr = File.ReadAllLines("input.txt");
            Part1(instr);
            Part2(instr, 119315717514047, 101741582076661);
        }

        private static void Part1(string[] instr)
        {
            int cardsCount = 10007;
            var (a,b) = FoldInstructions(instr, cardsCount);
            var res = OperationHelper.ProductModulo(OperationHelper.Modulo(2019-b, cardsCount), EuclidAlgorithm.Invert(a, cardsCount), cardsCount); 
            Console.WriteLine(res);
        }

        private static void Part2(string[] instr, long cardsCount, long shufleCount)
        {
            var (a,b) = FoldInstructions(instr, cardsCount);
            var c = new (long, long)[64];
            c[0] = (a,b);
            for(int i = 1; i < 64; i++)
                c[i] = OperationHelper.Fold(c[i-1], c[i-1], cardsCount);
            var f = (1L, 0L);
            int ind = 0;
            while(shufleCount > 0)
            {
                if(shufleCount % 2 == 1) 
                    f = OperationHelper.Fold(f, c[ind], cardsCount);
                ind++;
                shufleCount /= 2;
            }
            Console.WriteLine(OperationHelper.Modulo(OperationHelper.ProductModulo(f.Item1, 2020, cardsCount) + f.Item2, cardsCount));
        }

        private static (long,long) FoldInstructions(string[] instr, long cardsCount)
        {
            var fun = (1L, 0L);
            foreach(var ins in instr)
            {
                if(ins.StartsWith("deal into new stack"))
                    fun = OperationHelper.Fold((-1L, cardsCount-1), fun, cardsCount);
                else if(ins.StartsWith("cut "))
                    fun = OperationHelper.Fold((1L, long.Parse(ins.Substring(4))), fun, cardsCount);
                else if(ins.StartsWith("deal with increment "))
                {
                    var n = long.Parse(ins.Substring(20));
                    var nInv = OperationHelper.Modulo(EuclidAlgorithm.Invert(n, cardsCount), cardsCount);
                    fun = OperationHelper.Fold((nInv,0L), fun, cardsCount);
                }
            }
            return fun;
        }
    }
}