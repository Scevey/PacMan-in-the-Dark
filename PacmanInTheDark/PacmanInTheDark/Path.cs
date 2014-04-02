﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    enum Orientation { Horizontal, Vertical };
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

        //Whether it's on a horizontal or vertical path
        Orientation orientation;
        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
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
        
        //takes start and end
        public Path(Point _start, Point _end)
        {
            //sets start and end
            start = _start;
            end = _end;

            if (start.X != end.X)
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

            if (det == 0)
            {
                intersect = new Point(-1, -1);
                intersects = false;
            }
            else
            {
                intersect = new Point((B2 * C1 - B1 * C2) / det, (A1 * C2 - A2 * C1) / det);
                intersects = true;
            }
            return intersects;
        }

        //we may want to add more methods here, but I don't what else we'll need at this point
    }
}
