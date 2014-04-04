﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    abstract class GamePiece
    {
        #region Fields
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
            protected set
            {
                pathPos = value;
            }
        }

        //TODO add variable for spritesheet

        #endregion

        #region Properties
        public float PathTraversal
        {
            get
            {
                //number (0-1) representing how far along the path the piece is
                //obtained by dividing the current path position (which is a number of map tiles) by the path's length (which is also in map tiles)
                return PathPos / currentPath.Length;
            }
        }

        //field for the position of the piece in map-space
        //don't attempt to set this to a value, it isn't actually stored in memory. It isn't even a real variable
        public Point MapPos
        {
            get
            {
                //a vector representing the 2D offset of the piece from the start point of the path
                Point vectorOffset = currentPath.PathVector * PathTraversal;

                //adds the vector offset to the start point of the current path and returns the result
                return currentPath.Start + vectorOffset;
            }
        }

        #endregion

        //takes initial positional parameters
        public GamePiece(Path path, float pos) //TODO add parameter for spritesheet
        {
            currentPath = path;
            pathPos = pos;

            //TODO add declaration for spritesheet
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
