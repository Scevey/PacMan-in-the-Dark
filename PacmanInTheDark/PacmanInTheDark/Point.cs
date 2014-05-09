using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    struct Point //used to reference locations in map-space and to represent vectors
    {
        #region Fields

        //coords of the point
        float x;
        public float X
        {
            get
            {
                return x;
            }
        }
        float y;
        public float Y
        {
            get
            {
                return y;
            }
        }

        #endregion

        public Point(float _x, float _y) //takes coords, sets coords
        {
            x = _x;
            y = _y;
        }

        public static float Distance(Point p1, Point p2)
        {
            return (float)Math.Abs(Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y)));
        }

        //coord conversion
        //converts map coords to gameplay box coords
        public static Point MapToScreen(Point mapPoint, Point mapSize, Point screenSize)
        {
            return new Point((int)((mapPoint.X * screenSize.X) / mapSize.X), (int)((mapPoint.Y * screenSize.Y / mapSize.Y)));
        }

        #region operator overrides
        //these make Points behave like points and vectors from Math Graph

        //defines addition of two Points
        public static Point operator +(Point p1, Point p2)
        {
            //returns a Point whose components are the sums of the initial two Points' components
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        //defines subtraction of two Points
        public static Point operator -(Point p1, Point p2)
        {
            //returns a Point whose components are the differences between the initial two Points' components
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        //defines multiplication of a Point with a float
        public static Point operator *(float f1, Point p1)
        {
            //returns a Point whose components whose components are the products of the initial Point's components and the float
            return new Point(p1.X * f1, p1.Y * f1); 
        }

        //same as previous but in reverse order
        public static Point operator *(Point p1, float f1)
        {
            //returns a Point whose components whose components are the products of the initial Point's components and the float
            return new Point(p1.X * f1, p1.Y * f1);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return (p1.X == p2.X && p1.Y == p2.Y) ? true : false;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return (p1.X == p2.X && p1.Y == p2.Y) ? false : true;
        }

        #endregion
    }

    struct PathPosition
    {
        Path path;
        public Path _Path
        {
            get
            {
                return path;
            }
        }

        float position;
        public float Position
        {
            get
            {
                return position;
            }
        }

        public PathPosition(Path _path, float _position)
        {
            path = _path;
            position = _position;
        }

        public static implicit operator Point(PathPosition ppos)
        {
            //start point of the path plus the path vector times path position over path length
            return ppos.path.Start + (ppos.path.PathVector * (ppos.position / ppos.path.Length));
        }
    }

}
