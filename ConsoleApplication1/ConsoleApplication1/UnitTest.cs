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
    }
}
