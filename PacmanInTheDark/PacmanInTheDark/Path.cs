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
        Point end;

        //Dictionary of intersecting paths (elements) and the points of intersection (keys)
        Dictionary<Path, Point> intersectionDictionary;

        //attribute for length
        int length;
        public int Length
        {
            get
            {
                return length;
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

            intersect = null;
            return false;
        }

        //we may want to add more methods here, but I don't what else we'll need at this point
    }
}
