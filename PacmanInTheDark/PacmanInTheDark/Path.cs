
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    //Jeremy Hall
    enum Orientation { Horizontal, Vertical };
    class Path
    {
        #region Fields

        //start and end point for the path
        Point start;
        public Point Start
        {
            get
            {
                return start;
            }
        }

        Point end;
        public Point End
        {
            get
            {
                return end;
            }
        }

        //Whether it's on a horizontal or vertical path
        Orientation orientation;
        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        //Dictionary of intersecting paths (elements) and the points of intersection (keys)
        Dictionary<Path, Point> intersectionDictionary;
        public Dictionary<Path, Point> IntersectionDictionary
        {
            get
            {
                return intersectionDictionary;
            }
        }

        #endregion

        #region Properties

        //property for length
        //don't attempt to set this, it isn't stored in memory. It isn't even a real variable
        public float Length
        {
            get
            {
                //distance formula
                return Point.Distance(Start, End);
            }
        }

        //property for the path's vector representation
        public Point PathVector
        {
            get
            {
                //returns a Point that represents the path written as a vector
                //specifically, returns the difference between the start and end points
                return End-Start;
            }
        }

        #endregion

        //takes start and end
        public Path(Point _start, Point _end)
        {
            //sets start and end
            start = _start;
            end = _end;

            if (start.X == end.X)
            {
                orientation = Orientation.Vertical;
            }
            else
            {
                orientation = Orientation.Horizontal;

            }

            intersectionDictionary = new Dictionary<Path, Point>();
        }

        //takes a path, adds that path's info to the dictionary if it intersects. If not, does nothing
        public void AddIntersect(Path path)
        {
            Point intersectionPoint;

            //if paths intersect and path isn't already in the dictionary, add info to the dictionary
            if (this.Intersects(path, out intersectionPoint) && !intersectionDictionary.ContainsKey(path))
                intersectionDictionary.Add(path, intersectionPoint);
        }

        //takes path, returns true if it intersects this path, else returns false. outs the point of intersection
        public bool Intersects(Path path, out Point intersect)
        {
            bool intersects;

            //intersection maths
            float A1 = this.End.Y - this.Start.Y;
            float B1 = this.Start.X - this.End.X;
            float C1 = A1 * this.start.X + B1 * this.Start.Y;

            float A2 = path.End.Y - path.Start.Y;
            float B2 = path.Start.X - path.End.X;
            float C2 = A2 * path.start.X + B2 * path.Start.Y;

            float det = A1 * B2 - A2 * B1;

            intersects = false;

            if (det == 0)
            {
                intersect = new Point(-1, -1);
            }
            else
            {
                intersect = new Point((B2 * C1 - B1 * C2) / det, (A1 * C2 - A2 * C1) / det);

                if (PointOnPath(intersect, this) && PointOnPath(intersect, path))
                    intersects = true;
            }
            return intersects;
        }

        //determines whether a given path can be entered from this path from a given direction
        public bool PathEnterable(Path p, Direction dir)
        {
            //returns false if the given path and this one don't intersect
            if (!intersectionDictionary.ContainsKey(p))
            {
                return false;
            }

            if (p.Orientation == Orientation.Vertical)
            {
                if (dir == Direction.Down && IntersectionDictionary[p] != p.End)
                    return true;
                if (dir == Direction.Up && IntersectionDictionary[p] != p.Start)
                    return true;
            }
            else
            {
                if (dir == Direction.Right && IntersectionDictionary[p] != p.End)
                    return true;
                if (dir == Direction.Left && IntersectionDictionary[p] != p.Start)
                    return true;
            }

            return false;
        }

        //
        public Point OtherPoint(Point p)
        {
            if (p == this.Start)
                return this.End;
            else if (p ==this.End)
                return this.Start;

            return p;
        }

        static bool PointOnPath(Point p, Path path)
        {
            if (p.X == path.Start.X && p.X == path.End.X && p.Y >= path.Start.Y && p.Y <= path.End.Y) return true;
            if (p.Y == path.Start.Y && p.Y == path.End.Y && p.X >= path.Start.X && p.X <= path.End.X) return true;
            return false;
        }
    }
}
