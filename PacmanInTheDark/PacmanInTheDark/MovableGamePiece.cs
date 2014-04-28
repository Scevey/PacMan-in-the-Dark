﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    //Jeremy Hall
    //indicates current direction of movement, not desired direction of movement
    //user input will not directly control this
    enum Direction { None, Up, Left, Down, Right };

    abstract class MovableGamePiece : GamePiece
    {
        #region Fields

        // Current direction of pacman
        Direction direction;
        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        //the previous direction of movement
        Direction lastDirection;
        public Direction LastDirection
        {
            get
            {
                return lastDirection;
            }
            set
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
        }

        #endregion

        public MovableGamePiece(Path path, float pos, float _speed)
            : base(path, pos)
        {
            speed = _speed;
            if (path.Orientation == Orientation.Horizontal)
                direction = Direction.Left;
            else
                direction = Direction.Up;
        }

        //handles movement
        public void Move()
        {
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
            if (direction == Direction.None) return;

            if (CurrentPath.Orientation == Orientation.Horizontal)
            {
                if (direction == Direction.Left)
                {
                    //if the piece is within one move of the edge of the path...
                    if (PathPos < speed)
                    {
                        //... place it at the end of the path and stop its motion
                        PathPos = 0;
                        lastDirection = direction;
                        direction = Direction.None;
                        return;
                    }
                    PathPos -= speed;
                }
                else if (direction == Direction.Right)
                {
                    if (PathPos > (CurrentPath.Length - speed))
                    {
                        PathPos = CurrentPath.Length;
                        lastDirection = direction;
                        direction = Direction.None;
                        return;
                    }
                    PathPos += speed;
                }
                else
                {
                    direction = Direction.None;
                    return;
                }
            }

            else
            {
                if (direction == Direction.Down)
                {
                    if (PathPos > (CurrentPath.Length - speed))
                    {                        
                        PathPos = CurrentPath.Length;
                        lastDirection = direction;
                        direction = Direction.None;
                        return;
                    }
                    PathPos += speed;
                }
                else if (direction == Direction.Up)
                {                    
                    if (PathPos < speed)
                    {                        
                        PathPos = 0;
                        lastDirection = direction;
                        direction = Direction.None;
                        return;
                    }
                    PathPos -= speed;
                }
                else
                {
                    direction = Direction.None;
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
            direction = nextDirection;

            //note -- the overrun is necessary to preserve movement speed
            //without it the piece would lose a fraction of a movement at each path change
            //while it probably wouldn't be noticeable, this is cleaner

            if (direction == Direction.Left || direction == Direction.Up)
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
            CurrentPath = outWarp.CurrentPath;
            PathPos = outWarp.PathPos;
        }
    }
}
