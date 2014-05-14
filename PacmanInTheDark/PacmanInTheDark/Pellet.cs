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
    // Mike Teixeira
    // Sungmin Park (Draw related code)
    class Pellet : GamePiece
    {
        // Attributes
        bool active;
        bool isBigPellet;

        //Draw related attributes
        int frame = 4; // what frame to draw
        int numFrames = 4; // number of frames
        int frameSizeX = 100; // size of frame in pixels
        int frameSizeY = 100; // size of frame in pixels
        int timeSinceLastFrame = 0; // elapsed time on this frame
        int millisecondsPerFrame = 150; // how long to display a frame
        int currentFrameX; // location on spire sheet of the frame
        const int yPosOffSet = 157; // how off the pellet's y coordinate is from the map
        const int xPosOffSet = -5; // how off the pellet's x coordinate is from the map
        Vector2 pelletPos; // position of pellet

        // properties
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public bool IsBigPellet
        {
            get { return isBigPellet; }
            set { isBigPellet = value; }
        }

        // Constructor
        public Pellet(Path p, float pos)
            : base(p,pos)
        {
            this.active = true;
        }

        public void UpdateFrame(GameTime gameTime)
        {
            // increment the elapsed time
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            // time for the next frame
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                currentFrameX = 0;
                timeSinceLastFrame = 0; // reset elapsed time
                frame++;
                if (frame >= numFrames)
                {
                    frame = 0;
                }
                currentFrameX = frameSizeX * frame;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Point mapCoord, Point pixelCoord)
        {
        }

        // Draws the pellet to screen
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Point MapCoord, Point PixelCoord, Texture2D image)
        {
            //get to pelletPos coordinates
            Point location = new Point(0, 0);

            // convert path location to screen location
            location = Point.MapToScreen(MapPos, MapCoord, PixelCoord);

            // store converted points into a Vector2D
            pelletPos.X = location.X + xPosOffSet;
            pelletPos.Y = location.Y + yPosOffSet;

            if (IsBigPellet == true)
            {
                spriteBatch.Draw(image, new Rectangle((int)pelletPos.X, (int)pelletPos.Y, 40, 40), new Rectangle(currentFrameX, 0, frameSizeX, frameSizeY), Color.White);
            }
            else
            {
                spriteBatch.Draw(image, new Rectangle((int)(pelletPos.X + 10), (int)(pelletPos.Y + 15), 20, 20), Color.White);
            }
        }

        public override void Collision(Pacman pac)
        {
            //leave this line
            if (!active) return;

            // collides with pacman - becomes inactive
            this.active = false;

            // depending on the pellet add different amount of score and health
            if (IsBigPellet == true)
            {
                pac.Score += 250;
                pac.Health += 5f;
            }
            else
            {
                pac.Score += 50;
                pac.Health += .5f;
            }

            // makes sure health doesnt go over 100
            if (pac.Health >= 100)
            {
                pac.Health = 100;
            }
        }

        public override void Collision(Ghost ghost)
        {
        }
    }
}
