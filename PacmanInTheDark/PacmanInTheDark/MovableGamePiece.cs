using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    //Jeremy Hall
    // Mike Teixeira

    //indicates current direction of movement, not desired direction of movement
    //user input will not directly control this
    enum Direction { None, Up, Left, Down, Right };

    abstract class MovableGamePiece : GamePiece
    {
        #region Fields

        // Current direction of pacman
        Direction currentDirection;
        public Direction CurrentDirection
        {
            get { return currentDirection; }
            protected set { currentDirection = value; }
        }

        //the previous direction of movement
        Direction lastDirection;
        public Direction LastDirection
        {
            get
            {
                return lastDirection;
            }
            protected set
            {
                lastDirection = value;
            }
        }

        // Next direction that pacman will be going, used for next path
        Direction nextDirection;
        public Direction NextDirection
        {
            get { return nextDirection; }
            set { nextDirection = value; }
        }

        //speed variable
        float speed; //represents a number of tiles. It will be really small
        public float Speed
        {
            get
            {
                return speed;
            }
            protected set
            {
                speed = value;
            }
        }

        #endregion

        public MovableGamePiece(Path path, float pos, float _speed)
            : base(path, pos)
        {
            speed = _speed;
            if (path.Orientation == Orientation.Horizontal)
                currentDirection = Direction.Left;
            else
                currentDirection = Direction.Up;
        }

        //handles movement
        public void Move()
        {
            //allows collision at intersection points
            //checks every path intersecting with the current path
            foreach (Path p in CurrentPath.IntersectionDictionary.Keys)
            {
                //if the piece is within one move of the intersection point...
                if (Point.Distance(this.MapPos, CurrentPath.IntersectionDictionary[p]) <= this.speed)
                    //add it to that path's piece list
                    p.pieces.Add(this);
                else
                {
                    //otherwise, remove it
                    p.pieces.Remove(this);
                }
            }

            #region path change block - exits the method if a path change occurs

            // Retrieves nextDirection - set based off of user keyboard input or in ghost's case, random
            GetNextDirection();

            //iterates through all the paths intersecting with the current path
            //if the current position is within one movement of the intersection point and the direction to the path matches
            //the current nextDirection, a path change occurs
            foreach (Path p in CurrentPath.IntersectionDictionary.Keys)
            {

                //if next direction and the direction to the path match *and* the intersection is within one movement...
                if (CurrentPath.PathEnterable(p, nextDirection) && Point.Distance(MapPos, CurrentPath.IntersectionDictionary[p]) <= speed)
                {
                    //change path and end the move method
                    PathChange(p);
                    return;
                }
            }

            #endregion

            #region movement block - this block will not be reached if a path change occurs

            //does nothing if 
            if (currentDirection == Direction.None) return;

            if (CurrentPath.Orientation == Orientation.Horizontal)
            {
                if (currentDirection == Direction.Left)
                {
                    //if the piece is within one move of the edge of the path...
                    if (PathPos < speed)
                    {
                        //... place it at the end of the path and stop its motion
                        PathPos = 0;
                        lastDirection = currentDirection;
                        currentDirection = Direction.None;
                        return;
                    }
                    PathPos -= speed;
                }
                else if (currentDirection == Direction.Right)
                {
                    if (PathPos > (CurrentPath.Length - speed))
                    {
                        PathPos = CurrentPath.Length;
                        lastDirection = currentDirection;
                        currentDirection = Direction.None;
                        return;
                    }
                    PathPos += speed;
                }
                else
                {
                    currentDirection = Direction.None;
                    return;
                }
            }

            else
            {
                if (currentDirection == Direction.Down)
                {
                    if (PathPos > (CurrentPath.Length - speed))
                    {                        
                        PathPos = CurrentPath.Length;
                        lastDirection = currentDirection;
                        currentDirection = Direction.None;
                        return;
                    }
                    PathPos += speed;
                }
                else if (currentDirection == Direction.Up)
                {                    
                    if (PathPos < speed)
                    {                        
                        PathPos = 0;
                        lastDirection = currentDirection;
                        currentDirection = Direction.None;
                        return;
                    }
                    PathPos -= speed;
                }
                else
                {
                    currentDirection = Direction.None;
                    return;
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

            //sets path pos to the distance between the start point and intersect point
            PathPos = Point.Distance(newPath.Start, CurrentPath.IntersectionDictionary[newPath]);

            //sets direction to nextDirection
            currentDirection = nextDirection;

            //note -- the overrun is necessary to preserve movement speed
            //without it the piece would lose a fraction of a movement at each path change
            //while it probably wouldn't be noticeable, this is cleaner

            if (currentDirection == Direction.Left || currentDirection == Direction.Up)
                PathPos = PathPos - overrun;
            else
                PathPos = PathPos + overrun;

            //removes the piece from the current path's piece list
            CurrentPath.pieces.Remove(this);

            //sets the current path to the new path
            CurrentPath = newPath;

            //adds the piece to the new current path's piece list
            CurrentPath.pieces.Add(this);
        }

        //determines the direction in which the piece will be moving after the next path change
        public abstract void GetNextDirection();

        public void WarpCollision(Warp outWarp)
        {
            LastDirection = CurrentDirection;
            CurrentPath.pieces.Remove(this);
            CurrentPath = outWarp.CurrentPath;
            CurrentPath.pieces.Add(this);
            PathPos = outWarp.PathPos;
        }
    }
}
