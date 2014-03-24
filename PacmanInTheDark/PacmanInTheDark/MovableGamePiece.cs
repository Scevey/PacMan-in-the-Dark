using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    //enum Direction { Up, Left, Down, Right };

    abstract class MovableGamePiece : GamePiece
    {
        // Note from Jeremy -- use Direction enum instead of byte direction. **Will be easier to understand
        //Direction direction;

        //direction variable
        byte direction; //only ever 0-3, byte to save space

        //the next path the piece will move to
        Path nextPath;

        //speed variable
        float speed; //represents a number of tiles. It will be really small

        public MovableGamePiece(Path path, float pos)
            : base(path, pos)
        {
            //TODO
        }

        //handles movement
        public void Move()
        {
            //TODO
        }

        //move the piece from one path to another
        void PathChange(Path newPath)
        {
            //TODO
        }

        //determines the next path the piece will move to
        abstract Path GetNextPath();
    }
}
