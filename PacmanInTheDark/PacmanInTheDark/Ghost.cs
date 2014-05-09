using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace PacmanInTheDark
{
    //Jeremy Hall
    class Ghost : MovableGamePiece
    {
        // Attributes and properties
        #region
        //Random variable used to decide which path/direction ghost has
        Random randy = new Random();

        //Ghost image
        Texture2D ghostImg;
        public Texture2D GhostImg
        {
            get { return ghostImg; }
            set { ghostImg = value; }
        }

        Texture2D glowImg;
        public Texture2D GlowImg
        {
            get { return glowImg; }
            set { glowImg = value; }
        }
        // Is ghost slowed?
        bool isSlowed = false;
        public bool IsSlowed
        {
            get { return isSlowed; }
            set { isSlowed = value; }
        }

        // Used to retrieve original speed after ghost has been slowed
        float originalSpeed;
        public float OriginalSpeed
        {
            get { return originalSpeed; }
        }
        float originalPos; //starting position
        public float OriginalPos
        {
            get { return originalPos; }
        }
        Path originalPath; //starting path
        public Path OriginalPath
        {
            get { return originalPath; }
        }
        #endregion
        /// <summary>
        /// Constructor to create Ghost
        /// </summary>
        /// <param name="myGhostImg">Starting image</param>
        /// <param name="path">Starting path</param>
        /// <param name="pos">Starting pos. on path</param>
        public Ghost(Texture2D myGhostImg, Texture2D glow, Path path, float pos, float speed)
            : base(path, pos, speed)
        {
            //Starting information for ghost
            GhostImg = myGhostImg;
            originalSpeed = speed;
            originalPos = pos;
            originalPath = path;

            //Starting image for Ghost
            glowImg = glow;
            ghostImg = myGhostImg;
            frame = 2;
            currentFrameX = 0;
            currentFrameY = 0;
            frameSizeX = 100;
            frameSizeY = 100;
            millisecondsPerFrame = 75;
        }


        //Draw related attributes
        int frame; // what frame to draw
        int frameSizeX, frameSizeY; // size of frame in pixels
        int timeSinceLastFrame; // elapsed time on this frame
        int millisecondsPerFrame; // how long to display a frame
        int currentFrameX, currentFrameY; // location on spire sheet of the frame
        const int yPosOffSet = 155; // how off ghost's y coordinate is from the map
        const int xPosOffSet = -5; // how off ghost's x coordinate is from the map
        const int xglowOffSet = 50; // how off ghosts' glow x coord is from the map
        const int yglowOffset = 50; // how off ghosts' glow y coord is from the map
        Vector2 ghostPos; //position of ghost in pixels
        Vector2 glowPos; // position of ghost glow in pixels
        //updates the frame to display
        public void UpdateFrame(GameTime gameTime)
        {
            // increment the elapsed time
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            // time for the next frame
            if (timeSinceLastFrame > millisecondsPerFrame)
            {

                if (CurrentDirection == Direction.Up)
                {
                    timeSinceLastFrame = 0; // reset elapsed time
                    frame++;
                    if (frame % 2 == 0)
                    {
                        frame = 0;
                    }
                    else
                    {
                        frame = 1;
                    }
                    currentFrameX = frameSizeX * frame;
                }
                if (CurrentDirection == Direction.Right)
                {
                    timeSinceLastFrame = 0; // reset elapsed time
                    frame++;
                    if (frame % 2 == 0)
                    {
                        frame = 2;
                    }
                    else
                    {
                        frame = 3;
                    }
                    currentFrameX = frameSizeX * frame;
                }
                if (CurrentDirection == Direction.Down)
                {
                    timeSinceLastFrame = 0; // reset elapsed time
                    frame++;
                    if (frame % 2 == 0)
                    {
                        frame = 4;
                    }
                    else
                    {
                        frame = 5;
                    }
                    currentFrameX = frameSizeX * frame;
                }
                if (CurrentDirection == Direction.Left)
                {
                    timeSinceLastFrame = 0; // reset elapsed time
                    frame++;
                    if (frame % 2 == 0)
                    {
                        frame = 6;
                    }
                    else
                    {
                        frame = 7;
                    }
                    currentFrameX = frameSizeX * frame;
                }
                if (CurrentDirection == Direction.None)
                {
                }
            }
        }

        /// <summary>
        /// Update Ghost Image
        /// </summary>
        // Stub Draw
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Point MapCoord, Point PixelCoord)
        {
        }
        // Working draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Point MapCoord, Point PixelCoord, int frameY)
        {
            //get to ghostPos coordinates
            Point location = new Point(0, 0);

            // convert path location to screen location
            location = Point.MapToScreen(MapPos, MapCoord, PixelCoord);

            // store converted points into a Vector2D
            ghostPos.X = location.X + xPosOffSet;
            ghostPos.Y = location.Y + yPosOffSet;

            glowPos.X = ghostPos.X - xglowOffSet;
            glowPos.Y = ghostPos.Y - yglowOffset;

            spriteBatch.Draw(GlowImg, new Rectangle((int)glowPos.X, (int)glowPos.Y, 140, 150), new Rectangle(frameY, 0, frameSizeX, frameSizeY), Color.White);
            if (CurrentDirection == Direction.Up || (CurrentDirection == Direction.None && LastDirection == Direction.Up))
            {
                spriteBatch.Draw(ghostImg, new Rectangle((int)ghostPos.X, (int)ghostPos.Y, 40, 40), new Rectangle(currentFrameX, frameY, frameSizeX, frameSizeY), Color.White);
            }
            else if (CurrentDirection == Direction.Down || (CurrentDirection == Direction.None && LastDirection == Direction.Down))
            {
                spriteBatch.Draw(ghostImg, new Rectangle((int)ghostPos.X, (int)ghostPos.Y, 40, 40), new Rectangle(currentFrameX, frameY, frameSizeX, frameSizeY), Color.White);

            }
            else if (CurrentDirection == Direction.Left || (CurrentDirection == Direction.None && LastDirection == Direction.Left))
            {
                spriteBatch.Draw(ghostImg, new Rectangle((int)ghostPos.X, (int)ghostPos.Y, 40, 40), new Rectangle(currentFrameX, frameY, frameSizeX, frameSizeY), Color.White);
            }
            else if (CurrentDirection == Direction.Right || (CurrentDirection == Direction.None && LastDirection == Direction.Right))
            {
                spriteBatch.Draw(ghostImg, new Rectangle((int)ghostPos.X, (int)ghostPos.Y, 40, 40), new Rectangle(currentFrameX, frameY, frameSizeX, frameSizeY), Color.White);
            }
            else
            {
                spriteBatch.Draw(ghostImg, new Rectangle((int)ghostPos.X, (int)ghostPos.Y, 40, 40), new Rectangle(currentFrameX, frameY, frameSizeX, frameSizeY), Color.White);
            }
            
        }

        /// <summary>
        /// Pick ghost's next direction (RANDOM GHOST)
        /// </summary>
        public override void GetNextDirection()
        {
            //if the ghost isn't moving, reverse its direction
            if (CurrentDirection == Direction.None)
            {
                if (LastDirection == Direction.Up)
                    CurrentDirection = Direction.Down;
                if (LastDirection == Direction.Down)
                    CurrentDirection = Direction.Up;
                if (LastDirection == Direction.Left)
                    CurrentDirection = Direction.Right;
                if (LastDirection == Direction.Right)
                    CurrentDirection = Direction.Left;
            }

            bool validDir = false;

            //checks to see if the chosen direction is a valid exit direction from the current path
            foreach (Path p in this.CurrentPath.IntersectionDictionary.Keys)
            {
                if (CurrentPath.PathEnterable(p, NextDirection))
                    validDir = true;
            }

            //if it is, do nothing
            if (validDir)
                return;

            //otherwise, choose a new next direction
            int randomDirection = randy.Next(1, 5);
            switch (randomDirection) // Changes next direction randomly
            {
                case 1:
                    NextDirection = Direction.Up;
                    break;
                case 2:
                    NextDirection = Direction.Left;
                    break;
                case 3:
                    NextDirection = Direction.Down;
                    break;
                case 4:
                    NextDirection = Direction.Right;
                    break;
            }
        }

        ///// <summary>
        ///// Pick ghost's next direction (CHASING GHOST)
        ///// </summary>
        //public override void GetNextDirection()
        //{
        //
        //}

        /// <summary>
        /// Slow ghost down by a certain proportion
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public float SlowGhost()
        {
            float newSpeed = OriginalSpeed / 10;  //Subject to change...we can make the ratio whatever we want
            return newSpeed;
        }

        /// <summary>
        /// Collision with pacman
        /// </summary>
        /// <param name="pac"></param>
        public override void Collision(Pacman pac)
        {
            pac.Collision(this);
        }
        /// <summary>
        /// Collision with ghost
        /// </summary>
        /// <param name="ghost"></param>
        public override void Collision(Ghost ghost)
        {
            return;
        }
    }
}
