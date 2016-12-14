using System;
using System.Collections.Generic;
using System.Linq;


namespace BSHEEP
{
    struct Point
    {
        public int X;
        public int Y;
        public int index;
        public double angle;        //angle with respect to the first point (p0)
        public double distance;     //distance away from the first point (p0)
    }

    class Program
    {
        private static List<Point> sheep = new List<Point>();
        private static List<Point> borderSheep = new List<Point>();

        static void Main(string[] args)
        {
            int t = Int32.Parse(Console.ReadLine());
            while (t-- > -1)
            {
                Console.ReadLine();

                int n = Int32.Parse(Console.ReadLine());
                RunAlgorithm(n);
            }
        }

        private static void RunAlgorithm(int n)
        {

            for (int i = 0; i < n; i++)
            {
                string[] points = Console.ReadLine().Split(' ');
                sheep.Add(new Point() { X = Int32.Parse(points[0]), Y = Int32.Parse(points[1]), index = i + 1 });
            }

            PrintAnswer(RunConvexHull());
        }

        //find the next to top of a stack
        private static Point nextToTop(Stack<Point> S)
        {
            Point p = S.First();
            S.Pop();
            Point res = S.First();
            S.Push(p);
            return res;
        }

        //swap two points
        private static void swap(Point p1, Point p2)
        {
            Point temp = p1;
            p1 = p2;
            p2 = temp;
        }

        //squared of distance between p1 and p2
        private static int distSq(Point p1, Point p2)
        {
            return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
        }


        //find orientation of ordered triplet
        private static int orientation(Point p, Point q, Point r)
        {
            int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0) return 0;     // collinear
            return (val > 0) ? 1 : 2;   //clock or counter clockwise
        }

//sort an array of points with respect to the first point

        private static double RunConvexHull()
       {
            double circ = 0;

            //Bottom-most point
            Point bottomLeft = GetBottomLeftSheep();
            //Place bottom-most point at first position
            borderSheep.Add(bottomLeft);

//Sort n-1 points with respect to the first point
//A point p1 comes before p2 if p2 has a larger polar angle than p1


            //if 2 or more points make the same angle with p0,
            //remove all but the one that is farthest from p0
            int size = sheep.Count();
            for (int i = 0; i < size; i++)     //go through array 
            {
                for (int j = 0; j < size; j++) //compare to every element in an array
                {
                     if (sheep[i].angle == sheep[j].angle)
                    {
                        if (sheep[i].distance > sheep[j].distance)
                            sheep.RemoveAt(j);
                        else
                            sheep.RemoveAt(i);
                        size--;
                    }
                }
            }           
           

//create empty stack and push first 3 points to it
//process remaining n-3 points
//Now stack has the output points
           
                 
            //if list of border sheep of points has less than 3 points,
            //convex hull is not possible,
            if (borderSheep.Count() < 3)
            {
                borderSheep.Clear();    //clear border sheep so that we do not accidentally output values
                circ = 0;               //set circ to 0 since convex hull not posisble
            }

            return circ;
        }

        private static Point GetBottomLeftSheep()
        {
            Point bottomLeftSheep = borderSheep[0];

            foreach (Point s in borderSheep.Skip(1))
            {
                if (s.Y < bottomLeftSheep.Y) bottomLeftSheep = s;
                else if (s.Y == bottomLeftSheep.Y && s.X < bottomLeftSheep.X) bottomLeftSheep = s;
            }

            return bottomLeftSheep;
        }

        private static void PrintAnswer(double c)
        {
            Console.WriteLine(c);
            Console.WriteLine(string.Join(" ", GetSheepIndexes()));
            Console.WriteLine();

            ClearLists();
        }

        private static List<int> GetSheepIndexes()
        {
            int sheepIndex = borderSheep.Where(s => s.index == GetBottomLeftSheep().index).Select(s => s.index).Single();

            List<int> indexes = new List<int>();

            for (int i = 0; i < borderSheep.Count; i++)
            {
                indexes.Add(borderSheep[sheepIndex % borderSheep.Count].index);
            }

            return indexes;
        }

        private static void ClearLists()
        {
            sheep.Clear();
            borderSheep.Clear();
        }
    }
}
