using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    //indicates current direction of movement, not desired direction of movement
    //user input will not directly control this
    enum Direction { Up, Left, Down, Right };

    abstract class MovableGamePiece : GamePiece
    {
        // Note from Jeremy -- use Direction enum instead of byte direction. **Will be easier to understand
        Direction direction;
        //note from mike -- okay

        //direction variable
        //byte direction; //only ever 0-3, byte to save space

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
        public void Move(float speed)
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
