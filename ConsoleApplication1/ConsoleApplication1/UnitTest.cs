using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ConsoleApplication1
{
    class UnitTest
    {
        [TestCase(1, 1000000000, 1, Result = 999999999)]
        [TestCase(10, 85, 100, Result = 1)]
        [TestCase(10,85,30,Result = 3)]
        public int Test(int X,int Y, int D)
        {
            return Solution.solution_FrogJump(X, Y, D);
        }

        [TestCase(5, new[] { 3, 4, 4, 6, 4, 4, 4 }, Result = new[] { 2, 2, 2, 5, 2 })]
        [TestCase(5, new[] { 1, 1, 1, 6, 1 }, Result = new[] { 4,3,3,3,3})]
        [TestCase(5, new[] { 5 }, Result = new[] { 0,0,0,0,1 })]
        [TestCase(5, new[] { 3, 4, 4, 6, 1, 4, 6 }, Result = new[] { 3, 3, 3, 3, 3 })]
        [TestCase(5, new[] { 3, 4, 4, 6, 1, 4, 4 }, Result = new[] { 3, 2, 2, 4, 2 })]
        public int[] Test2(int N, int[] A)
        {
            return Solution.solution_MaxCounters(N, A);
        }

        [TestCase(new[] { 8, 6, 5, 3, 2, 4, 7 }, new[] { 1, 1, 1, 1, 1, 0, 0 },1, Result = 1)]
        [TestCase(new[] { 4, 3, 2, 1, 5 }, new[] { 0, 1, 0, 0, 0 }, 2, Result = 2)]
        [TestCase(new[] { 4, 3, 2, 1, 5 }, new[] { 0, 1, 0, 1, 0 }, 3, Result = 2)]
        [TestCase(new[] { 4, 3, 2, 1, 5 }, new[] { 0, 0, 0, 0, 0 }, 4, Result = 5)]
        [TestCase(new[] { 4, 3, 2, 1, 5 }, new[] { 1, 1, 1, 1, 1 }, 5, Result = 5)]
        [TestCase(new[] { 4, 3, 2, 1, 5 }, new[] { 0, 0, 0, 1, 1 }, 6, Result = 5)]
        [TestCase(new[] { 4, 3, 2, 1, 5 }, new[] { 0, 0, 0, 1, 1 }, 7, Result = 5)]
        [TestCase(new[] { 5, 3, 2, 1, 4 }, new[] { 1, 0, 0, 0, 0 }, 8, Result = 1)]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 1, 1, 1, 0 }, 9, Result = 1)]
        public int TestFishAlive(int[] A, int[] B, int testnum)
        {
            return FishAlive.solution_FishAlive2(A, B);
        }

        [TestCase(new[] { 8 }, 1, Result = 1)]
        [TestCase(new[] { 8, 1, 1, 8 }, 2, Result = 3)]
        [TestCase(new[] { 8, 8, 8,8 }, 3, Result = 1)]
        [TestCase(new[] { 5, 7, 9, 8, 7, 4}, 4, Result = 5)]
        [TestCase(new[] { 5, 7, 9, 8, 7, 4, 5 }, 5, Result = 6)]
        [TestCase(new[] { 1000000000,999999999 }, 6, Result = 2)]
        [TestCase(new[] { 4, 7,9,7,4 }, 7, Result = 3)]
        [TestCase(new[] { 8, 8, 5, 7, 9, 8, 7, 4, 8 },8, Result = 7)]
        [TestCase(new[] { 8, 1, 8 }, 9, Result = 3)]
        [TestCase(new[] { 8, 1, 8, 2 }, 10, Result = 4)]
        [TestCase(new[] { 1, 4, 3, 4, 1, 4, 3, 4, 1 }, 11, Result = 7)]
        public int TestStoneWall(int[] H,int testnum)
        {
            return ConsoleApplication1.Solution.solution_Stonewall(H);
        }

        [TestCase(new[] { 4,3,4,4,4,2 }, 1, Result = 2)]
        [TestCase(new[] { 1,1,2,2,2,2}, 2, Result = 1)]
        public int TestEquiLeader(int[] A, int testnum)
        {
            return ConsoleApplication1.Solution.solution_EquiLeader(A);
        }

        [TestCase(new[] { 4, 3, 4, 4, 4, 2 }, 1, Result = 0)]
        [TestCase(new[] { 3,4,3,2,3,-1,3,3 }, 2, Result = 0)]
        [TestCase(new[] { 1, 2, 3, 4, 5, -1, -2, -3 }, 3, Result = -1)]
        [TestCase(new[] { 1, 2, 2147483647, 4, 5, -1, -2, -2147483648}, 4, Result = -1)]
        [TestCase(new[] { 0}, 4, Result = 0)]
        public int TestDominator(int[] A, int testnum)
        {
            return ConsoleApplication1.Solution.solution_Dominator(A);
        }

        [TestCase(new[] { 23171, 21011, 21123, 21366, 21013, 21367 }, 1, Result = 356)]
        [TestCase(new[] { 21123, 23171, 21122, 24171, 21013, 21367 }, 2, Result = 3049)]
        [TestCase(new[] { 20122, 24170, 21123, 23171, 21122, 24171, 21013, 21367 }, 3, Result = 4049)]
        [TestCase(new[] { 23122, 23121, 23120, 23119, 23118, 23117, 23116, 23115 }, 4, Result = 0)]
        [TestCase(new[] { 23115, 23116, 23117, 23118, 23119, 23120, 23121, 23122 }, 5, Result = 7)]
        [TestCase(new[] { 99, 1, 2, 0, 7, 5, 6 }, 6, Result = 7)]
        [TestCase(new int[]{}, 7, Result = 0)]
        public int TestMaxStockProfit(int[] A, int testnum)
        {
            return ConsoleApplication1.Solution.solution_MaxStockProfit(A);
        }


        [TestCase(1000000000, 1, Result = 126500)]
        [TestCase(30, 2, Result = 22)]
        [TestCase(25, 3, Result = 20)]
        [TestCase(5, 4, Result = 12)]
        [TestCase(1, 5, Result = 4)]
        public int TestMinParamRectangle(int N, int testnum)
        {
            return Solution.solution_MinParamRectangle(N);
        }

        [TestCase(2147483647, 1, Result = 2)]
        [TestCase(24, 2, Result = 8)]
        [TestCase(25, 3, Result = 3)]
        [TestCase(5, 4, Result = 2)]
        [TestCase(1, 5, Result = 1)]
        public int TestCountFactors(int N, int testnum)
        {
            return Solution.solution_CountFactors(N);
        }

        [TestCase(50000, new[] { 1, 4, 16 }, new[] { 50000, 10, 20 }, 1, Result = new[] { 12110, 4, 0 })]
        [TestCase(26, new[] { 1, 4, 16 }, new[] { 26, 10, 20 }, 2, Result = new[] { 10,4,0 })]
        [TestCase(1, new[] { 1, 1, 1 }, new[] { 1, 1, 1 }, 3, Result = new[] { 0,0,0 })]
        public int[] TestSemiPrime(int N, int[] P, int[] Q, int testnum)
        {
            return Solution.solution_SemiPrime(N, P, Q);
        }

        [TestCase(1, 1, 1, Result = 1)]
        [TestCase(1000000000, 2, 2, Result = 500000000)]
        [TestCase(1000000000, 3, 3, Result = 1000000000)]
        [TestCase(1000000000, 6, 4, Result = 500000000)]
        [TestCase(20000000, 5, 5, Result = 4000000)]
        [TestCase(10, 3, 6, Result = 10)]
        [TestCase(20, 3, 7, Result = 20)]
        public int TestChocolateCnt(int N, int M, int testnum)
        {
            return Solution.solution_ChocolatesCount(N, M);
        }

        [TestCase(new[] {4, 4, 5, 5, 1}, new[] {3, 2, 4, 3, 1}, 1, Result = new[] {5, 1, 8, 0, 1})]
        [TestCase(new[] { 30000 }, new[] { 1 }, 2, Result = new[] { 1 })]
        [TestCase(new[] { 30000 }, new[] { 30 }, 3, Result = new[] { 154758433 })]
        [TestCase(new[] { 1 }, new[] { 1 }, 4, Result = new[] { 1 })]
        public int[] TestLadderClimbFibonacci(int[] A, int[] B, int testnum)
        {
            return Solution.solution_FibonacciLadder(A, B);
        }

        [TestCase(new[] { -2, -2, -2, -1, -1, -1 }, 5, Result = 2)]
        [TestCase(new[] { -2,-1,0, 1, 2,2 }, 4, Result = 3)]
        [TestCase(new[] { 0, 1, 2, 3, 4, 5, 6, 6 }, 3, Result = 7)]
        [TestCase(new[] { 0,1,2,3,4,5,6,7 }, 3, Result = 8)]
        [TestCase(new[] { -6, -5, -4, -3, -2, -1 }, 3, Result = 6)]
        [TestCase(new[] { -5, -4, -4, 0, 3, 3 }, 2, Result = 4)]
        [TestCase(new[] { -5, -3, -1, 0, 3, 6 }, 1, Result = 5)]
        [TestCase(new[] { -2147483648 }, 6, Result = 1)]
        public int TestDistinctAbs(int[] A, int testnum)
        {
            return Solution.solution_DistinctAbs3(A);
        }

    }
}
