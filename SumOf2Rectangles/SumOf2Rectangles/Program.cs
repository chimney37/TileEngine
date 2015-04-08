using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

//Coordinate system: Assume an y-axis going from bottom of screen to upper and x-axis going from left of screen to right
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

    //orthogonal lines only
    public class Line
    {
        public int x { get; set; }
        public int y { get; set; }

        public Line()
        {
            x = 0;
            y = 0;
        }

        public override string ToString()
        {
            return string.Format("(x={0},y={1})",x,y);
        }
    }

    //clockwise direction
    public class Edge
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public int GetLengthSquare()
        {
            return Math.Abs(EndPoint.y*EndPoint.y - EndPoint.x*EndPoint.x);
        }

        public Line GetLine()
        {
            //check for zero length edges
            if (GetLengthSquare() != 0)
            {
                //horizontal line
                if (EndPoint.x != StartPoint.x && EndPoint.y == StartPoint.y)
                {
                    return new Line() {x = 0, y = EndPoint.y};
                }

                //vertical line
                if (EndPoint.y != StartPoint.y && EndPoint.x == StartPoint.x)
                {
                    return new Line() { x = EndPoint.x, y = 0};
                }
            }
            return null;
        }
    }

    public class Rect
    {
        public Point Ul { get; set; }
        public Point Lr { get; set; }

        public static bool Intersects(Rect a, Rect b)
        {
            return (a.Ul.x <= b.Lr.x &&
                a.Ul.y >= b.Lr.y &&
                a.Lr.x >= b.Ul.x &&
                a.Lr.y <= b.Ul.y);
        }

        public static bool Intersects(Rect a, Point b)
        {
            //boundary condition : equal values does not mean intersect
            return XIntersects(a, b) && YIntersects(a, b);
        }

        private static bool XIntersects(Rect a, Point b)
        {
            return (a.Ul.x < b.x && b.x < a.Lr.x);
        }

        private static bool YIntersects(Rect a, Point b)
        {
            return (a.Lr.y < b.y && b.y < a.Ul.y);
        }

        public static Point Intersects(Rect a, Edge b)
        {
            //vertical line
            if (b.GetLine().x != 0 && b.GetLine().y == 0)
            {
                //if intersect start point
                if (Intersects(a, new Point(){x = b.StartPoint.x, y = b.StartPoint.y}))
                {
                    return new Point(){x = b.GetLine().x, y=a.Ul.y};
                }

                //if intersect end point
                if (Intersects(a, new Point() { x = b.EndPoint.x, y = b.EndPoint.y }))
                {
                    return new Point() { x = b.GetLine().x, y = a.Lr.y };
                }
            }
            //horizontal line
            else if (b.GetLine().x == 0 && b.GetLine().y != 0)
            {
                //if intersect start point
                if (Intersects(a, new Point() { x = b.StartPoint.x, y = b.StartPoint.y }))
                {
                    return new Point() { x = b.GetLine().x, y = a.Ul.y };
                }

                //if intersect end point
                if (Intersects(a, new Point() { x = b.EndPoint.x, y = b.EndPoint.y }))
                {
                    return new Point() { x = b.GetLine().x, y = a.Lr.y };
                }
            }
            //invalid edge
            return null;
        }

        public int GetArea()
        {
            return GetWidth() * GetHeight();
        }

        public int GetWidth()
        {
            return (Lr.x - Ul.x);
        }

        public int GetHeight()
        {
            return (Ul.y - Lr.y);
        }

        public List<Point> GetVertices()
        {
            Point lr = new Point() {x = Lr.x, y = Lr.y};
            Point ll = new Point() {x = Ul.x, y = Lr.y};
            Point ur = new Point() {x = Lr.x, y = Ul.y};
            Point ul = new Point() {x = Ul.x, y = Ul.y};

            //Vertices are defined clockwise
            return new List<Point>()
            {
                lr,
                ll,
                ul,
                ur
            };
        }

        public List<Edge> GetEdges()
        {
            //edges are defined clockwise: starting from lower (bottom) edge
            Edge lower = new Edge() { StartPoint = new Point() { x = Lr.x, y = Lr.y }, EndPoint = new Point() { x = Ul.x, y = Lr.y } };
            Edge left = new Edge() { StartPoint = new Point() { x = Ul.x, y = Lr.y }, EndPoint = new Point(){x = Ul.x, y = Ul.y }};
            Edge upper = new Edge() { StartPoint = new Point() { x = Ul.x, y = Ul.y }, EndPoint = new Point(){x = Lr.x, y = Ul.y }};
            Edge right = new Edge() { StartPoint = new Point() { x = Lr.x, y = Ul.y }, EndPoint = new Point() { x = Lr.x, y = Lr.y } };

            return new List<Edge>()
            {
                lower,
                left,
                upper,
                right
            };
        }

        public static List<Point> GetVertexIntersections(Rect a, Rect b)
        {
            List<Point> pts = new List<Point>();
            foreach (Point p in a.GetVertices())
            {
                if(Intersects(b,p))
                    pts.Add(p);
            }

            foreach (Point p in b.GetVertices())
            {
                if(Intersects(a, p))
                    pts.Add(p);
            }

            return pts;
        }

        public static List<Line> GetEdgeIntersections(Rect a, Rect b)
        {
            List<Line> edges = new List<Line>();
            return edges;
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

            Console.WriteLine("Rectangle overlap = {0}", Rect.Intersects(a,b) );
            Console.WriteLine("Rectangle A area = {0}", a.GetArea());
            Console.WriteLine("Rectangle B area = {0}", b.GetArea());

            Console.Write("Vertices from A=");
            a.GetVertices().ForEach(Console.Write);
            Console.WriteLine();

            Console.Write("Edges from A=");
            a.GetEdges().ForEach(Console.Write);
            Console.WriteLine();

            Console.WriteLine("Vertices from A in B or B in A");
            Rect.GetVertexIntersections(a,b).ForEach(Console.Write);
            Console.WriteLine();



            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
