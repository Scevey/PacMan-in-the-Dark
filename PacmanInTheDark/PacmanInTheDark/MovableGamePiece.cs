using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    //indicates current direction of movement, not desired direction of movement
    //user input will not directly control this
    enum Direction { None, Up, Left, Down, Right };

    abstract class MovableGamePiece : GamePiece
    {
        #region Fields

        // Note from Jeremy -- use Direction enum instead of byte direction. **Will be easier to understand
        Direction direction;
        Direction nextDirection;
        //note from mike -- okay

        //direction variable
        //byte direction; //only ever 0-3, byte to save space

        //speed variable
        float speed; //represents a number of tiles. It will be really small

        #endregion

        public MovableGamePiece(Path path, float pos, float _speed)
            : base(path, pos)
        {
            speed = _speed;
        }

        //handles movement
        public void Move()
        {
            //TODO
            //iterates through all the paths intersecting with the current path
            //if the current position is within one movement of the intersection point and the direction to the path matches
            //the current nextDirection, a path change occurs
            //Note from Mike -- this isn't finished yet, and I have no time now. Comment it out of you need to, but don't alter it
            foreach (Path p in CurrentPath.IntersectionDictionary.Keys)
            {
                //if (nextDirection == CurrentPath.DirectionToPath(p) && MapPos>= CurrentPath.IntersectionDictionary[p]-
            }

            if (CurrentPath.Orientation == Orientation.Horizontal)
            {
                if (direction == Direction.Left)
                {
                    PathPos -= speed;
                }
                else
                {
                    PathPos += speed;
                }
            }
            else
            {
                if (direction == Direction.Down)
                {
                    PathPos += speed;
                }
                else
                {
                    PathPos -= speed;
                }
            }
        }

        //move the piece from one path to another
        void PathChange(Path newPath)
        {
            //TODO
        }

        //determines the next path the piece will move to
        public abstract Path GetNextDirection();
        
        
    }
}
