using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    class Path
    {
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

        //Dictionary of intersecting paths (elements) and the points of intersection (keys)
        Dictionary<Path, Point> intersectionDictionary;

        //property for length
        //don't attempt to set this, it isn't stored in memory. It isn't even a real variable
        public float Length
        {
            get
            {
                //distance formula
                return (float)Math.Sqrt((End.X-Start.X)*(End.X-Start.X)+(End.Y-Start.Y)*(End.Y-Start.Y));
            }
        }

        //field for the path's vector representation
        public Point PathVector
        {
            get
            {
                //returns a Point that represents the path written as a vector
                //specifically, each component of the point is the difference between the end and start points of the path
                return new Point(this.End.X - this.Start.X, this.End.Y - this.Start.Y);
            }
        }
        
        //takes start and end
        public Path(Point _start, Point _end)
        {
            //sets start and end
            start = _start;
            end = _end;
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
            Point intersectionPoint;

            //TODO intersection maths

            intersect = new Point(-1,-1);
            return false;
        }

        //we may want to add more methods here, but I don't what else we'll need at this point
    }
}
