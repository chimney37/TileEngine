using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace SumOf2Rectangles
{
    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }

        public override string ToString()
        {
            return string.Format("({0},{1})", x, y);
        }
    }

    public class Rect
    {
        //Rectangle class : Assume an y-axis going from bottom of screen to upper and x-axis going from left of screen to right
        public Point Ul { get; set; }
        public Point Lr { get; set; }

        public static bool Overlap(Rect a, Rect b)
        {
            return (a.Ul.x <= b.Lr.x &&
                a.Ul.y >= b.Lr.y &&
                a.Lr.x >= b.Ul.x &&
                a.Lr.y <= b.Ul.y);
        }

        public static bool Intersect(Rect a, Point b)
        {
            //boundary condition : equal values => interesect
            return (a.Ul.x <= b.x && b.x <= a.Lr.x) && (a.Lr.y <= b.y && b.y <= a.Ul.y);
        }

        public int GetArea()
        {
            return (Lr.x - Ul.x)*(Ul.y - Lr.y);
        }

        public List<Point> GetVertices()
        {
            Point lr = new Point() {x = Lr.x, y = Lr.y};
            Point ll = new Point() {x = Ul.x, y = Lr.y};
            Point ur = new Point() {x = Lr.x, y = Ul.y};
            Point ul = new Point() {x = Ul.x, y = Ul.y};

            return new List<Point>()
            {
                lr,
                ll,
                ul,
                ur
            };
        }

        public static List<Point> GetIntersections(Rect a, Rect b)
        {
            List<Point> pts = new List<Point>();
            foreach (Point p in a.GetVertices())
            {
                if(Intersect(b,p))
                    pts.Add(p);
            }

            foreach (Point p in b.GetVertices())
            {
                if(Intersect(a, p))
                    pts.Add(p);
            }

            return pts;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Rect a = new Rect()
            {
                Lr = new Point() { x = 2, y = 0 },
                Ul = new Point() { x = 1, y = 3 }
            };

            Rect b = new Rect()
            {
                Lr = new Point() {x = 3, y = 1},
                Ul = new Point() {x =0, y= 2}
            };

            Point pt1 = new Point() {x = 2, y = 1};

            Console.WriteLine("Rectangle overlap = {0}", Rect.Overlap(a,b) );
            Console.WriteLine("Rectangle A area = {0}", a.GetArea());
            Console.WriteLine("Rectangle B area = {0}", b.GetArea());
            Console.WriteLine("Rectangle B intersects Point pt1 = {0}", Rect.Intersect(b, pt1));

            Console.WriteLine("Vertices from A=");
            foreach(Point p in a.GetVertices())
                Console.Write(p);
            Console.WriteLine();

            Console.WriteLine("Vertices from A in B or B in A");
            foreach(Point p in Rect.GetIntersections(a,b))
                Console.Write(p);
            Console.WriteLine();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
