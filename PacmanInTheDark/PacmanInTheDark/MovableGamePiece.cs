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
            #region path change block - exits the method if a path change occurs

            //iterates through all the paths intersecting with the current path
            //if the current position is within one movement of the intersection point and the direction to the path matches
            //the current nextDirection, a path change occurs
            foreach (Path p in CurrentPath.IntersectionDictionary.Keys)
            {
                //if next direction and the direction to the path match *and* the intersection is within one movement...
                if (nextDirection == CurrentPath.DirectionToPath(p) && Point.Distance(MapPos, CurrentPath.IntersectionDictionary[p]) <= speed)
                {
                    //change path and end the move method
                    PathChange(p);
                    return;
                }
            }

            #endregion

            #region movement block - this block will not be reached if a path change occurs

            if (CurrentPath.Orientation == Orientation.Horizontal)
            {
                if (direction == Direction.Left)
                {
                    //if the piece is within one move of the edge of the path...
                    if (PathPos < speed)
                    {
                        //...turn it around
                        PathPos = speed - PathPos; //this positions the piece so as to make it seem like it had turned around at the end of the path rather than within one movement of the end
                        direction = Direction.Right;
                        return;
                    }
                    PathPos -= speed;
                }
                else
                {
                    if(PathPos>(CurrentPath.Length-speed))
                    {
                        //CurrentPath.Length-PathPos is the "remaining length" of the path
                        //speed - "remaining length" is how far the piece would be from the end of the path if it had continued to the end of the path before turning around ("overrun")
                        //CurrentPath.Length - "overrun" is what PathPos must be set to to account for the overrun
                        //I'll simplify this later
                        PathPos = CurrentPath.Length-(speed-(CurrentPath.Length-PathPos));
                        direction = Direction.Left;
                        return;
                    }
                    PathPos += speed;
                }
            }

            else
            {
                if (direction == Direction.Down)
                {
                    if (PathPos > (CurrentPath.Length - speed))
                    {
                        //CurrentPath.Length-PathPos is the "remaining length" of the path
                        //speed - "remaining length" is how far the piece would be from the end of the path if it had continued to the end of the path before turning around ("overrun")
                        //CurrentPath.Length - "overrun" is what PathPos must be set to to account for the overrun
                        //I'll simplify this later
                        PathPos = CurrentPath.Length - (speed - (CurrentPath.Length - PathPos));
                        direction = Direction.Up;
                        return;
                    }
                    PathPos += speed;
                }
                else
                {
                    //if the piece is within one move of the edge of the path...
                    if (PathPos < speed)
                    {
                        //...turn it around
                        PathPos = speed - PathPos; //this positions the piece so as to make it seem like it had turned around at the end of the path rather than within one movement of the end
                        direction = Direction.Down;
                        return;
                    }
                    PathPos -= speed;
                }
            }

            #endregion
        }

        //move the piece from one path to another
        void PathChange(Path newPath)
        {
            //get the distance to the intersection point
            float distanceToIntersect = Point.Distance(MapPos, CurrentPath.IntersectionDictionary[newPath]);

            //determine the difference between this distance and a single movement
            float overrun = speed - distanceToIntersect;

            //determines whether the intersection point of the new path is the start or end, and sets pathPos accordingly
            if (CurrentPath.IntersectionDictionary[newPath] == newPath.Start)
                //the piece is moved in the proper direction by the overrun
                PathPos = 0 + overrun;
            else
                PathPos = newPath.Length - overrun;

            //note -- the overrun is necessary to preserve movement speed
            //without it the piece would lose a fraction of a movement at each path change
            //while it probably wouldn't be noticeable, this is cleaner

            //sets the current path to the new path
            CurrentPath = newPath;
        }

        //determines the direction in which the piece will be moving after the next path change
        public abstract Path GetNextDirection();       
    }
}
