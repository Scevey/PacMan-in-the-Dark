using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    struct Point //used to reference locations in map-space
    {
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

        public Point(float _x, float _y) //takes coords, sets coords
        {
            x = _x;
            y = _y;
        }
    }
}
