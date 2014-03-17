using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    abstract class GamePiece
    {
        //path the piece is on
        Path currentPath;
        public Path CurrentPath
        {
            get
            {
                return currentPath;
            }
            set
            {
                currentPath = value;
            }
        }

        //position (in map tiles) of the piece on the path
        float pathPos;
        public float PathPos
        {
            get
            {
                return pathPos;
            }
            set
            {
                pathPos = value;
            }
        }

        //TODO add variable for spritesheet

        //takes positional parameters
        public GamePiece(Path path, float pos) //TODO add parameter for spritesheet
        {
            currentPath = path;
            pathPos = pos;

            //TODO add declaration for spritesheet
        }

        abstract void Draw();
    }
}
