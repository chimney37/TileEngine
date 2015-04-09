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
            return Solution.solution(X, Y, D);
        }

        [TestCase(5, new[] { 3, 4, 4, 6, 4, 4, 4 }, Result = new[] { 2, 2, 2, 5, 2 })]
        [TestCase(5, new[] { 1, 1, 1, 6, 1 }, Result = new[] { 4,3,3,3,3})]
        [TestCase(5, new[] { 5 }, Result = new[] { 0,0,0,0,1 })]
        [TestCase(5, new[] { 3, 4, 4, 6, 1, 4, 6 }, Result = new[] { 3, 3, 3, 3, 3 })]
        [TestCase(5, new[] { 3, 4, 4, 6, 1, 4, 4 }, Result = new[] { 3, 2, 2, 4, 2 })]
        public int[] Test2(int N, int[] A)
        {
            return Solution.solution6(N, A);
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

    }
}
