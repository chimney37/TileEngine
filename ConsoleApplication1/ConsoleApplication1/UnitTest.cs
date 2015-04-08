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

    }
}
