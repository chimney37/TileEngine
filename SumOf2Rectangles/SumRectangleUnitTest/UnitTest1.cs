using System;
using System.Runtime.CompilerServices;
using SumOf2Rectangles;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace SumRectangleUnitTest
{
    [TestFixture]
    public class UnitTest1
    {
        [TestCase(2, 0, 1, 3, 3, 1, 0, 2)]         //this case happens when no vertices overlap but the edges cross each other
        [TestCase(3, 1, 0, 2,2, 0, 1, 3 )]  //same as above but rects interchanged
        [TestCase(4,1,2,3,3,2,1,4)] //for single vertex overlap
        [TestCase(3,2,1,4,4,1,2,3)] //same as above but rects interchanged
        [TestCase(4, 2, 1, 4, 3, 1, 2, 3)]  //for 2 vertices overlap
        [TestCase(3, 1, 2, 3,4, 2, 1, 4)]  //same a above but rects interchanged
        public void TestRectsOverlap(int aLrX, int aLrY,int aUlX,int aUlY, int bLrX, int bLrY,int bUlX,int bUlY)
        {
            Rect a = new Rect()
            {
                Lr = new Point() { x = aLrX, y = aLrY },
                Ul = new Point() { x = aUlX, y = aUlY }
            };

            Rect b = new Rect()
            {
                Lr = new Point() { x = bLrX, y = bLrY },
                Ul = new Point() { x = bUlX, y = bUlY }
            };

            Assert.AreEqual(true, Rect.Intersects(a, b));

        }
    }
}
