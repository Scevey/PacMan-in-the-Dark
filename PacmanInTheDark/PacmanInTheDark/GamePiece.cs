using Microsoft.Xna.Framework;
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
        //or don't, I guess we don't need it

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

        //property for the position of the piece in map-space
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
            currentPath.pieces.Add(this);

            //TODO add declaration for spritesheet
        }

        /// <summary>
        /// Draws the piece to the screen or something
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="topLeft"></param>
        /// <param name="bottomRight"></param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Point topLeft, Point bottomRight);

        /// <summary>
        /// Called when the piece collides with pacman
        /// </summary>
        /// <param name="pac">The variable for the pacman the piece collided with</param>
        public abstract void PacmanCollision(Pacman pac);
    }
}
