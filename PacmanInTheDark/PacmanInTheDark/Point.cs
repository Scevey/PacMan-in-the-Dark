using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    class Point //only used to cleanly create paths, importing the actual Point class is overkill
    {
        //coords of the point
        int x;
        int y;

        public Point(int _x, int _y) //takes coords, sets coords
        {
            x = _x;
            y = _y;
        }
    }
}
