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
    //Sungmin Park
    class Pacman : MovableGamePiece
    {
        // Life/hunger bar (Lost over time - can be restored with pellets)
        int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        // How many times you can die (start with 3)
        int lives;
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        //Pacman image 
        Texture2D pacmanImg;
        public Texture2D PacmanImg
        {
            get { return pacmanImg; }
            set { pacmanImg = value; }
        }

        //Draw related attributes
        int frame; // what frame to draw
        int frameSizeX, frameSizeY; // size of frame in pixels
        int numFrames; // # of frames on the spirte sheet in a row
        int timeSinceLastFrame; // elapsed time on this frame
        int millisecondsPerFrame; // how long to display a frame
        int currentFrameX, currentFrameY; // location on spire sheet of the frame
        Vector2 pacmanPos; //position of PacMan in pixels

        byte imageReset = 0;

        //Keyboard state
        KeyboardState kState;

        /// <summary>
        /// Constructor to create Pacman Object
        /// </summary>
        /// <param name="myHealth">Starting health</param>
        /// <param name="myLives">Starting lives</param>
        /// <param name="path">Starting path</param>
        /// <param name="pos">Starting pos on path</param>
        public Pacman(Texture2D myPacman, Path path, float pos, float speed)
            : base(path, pos, speed)
        {
            //Pacman starts with 100 health and 3 lives
            Health = 100;
            Lives = 3;
            
            //Starting image for pacman
            PacmanImg = myPacman;
            numFrames = 3;
            frame = numFrames;
            currentFrameX = 0;
            currentFrameY = 0;
            frameSizeX = 100;
            frameSizeY = 100;
            millisecondsPerFrame = 25;
        }

        /// <summary>
        /// Used to decrease Pacman health
        /// </summary>
        /// <param name="decreaseHealthBy">Amount of health lost</param>
        public void LoseHealth(int healthLost)
        {
            if (Health > 0) Health -= healthLost;
            else this.LoseLife();
        }
        
        /// <summary>
        /// When Pacman runs out of health, subtract a life
        /// </summary>
        public void LoseLife()
        {
            if (Lives > 0)
            {
                lives -= 1;
                Health = 100;
                //TODO
                //Reset Pacman pos. and path               
            }
            //else gameState.Gameover();
        }

        //updates the frame to display
        public void UpdateFrame(GameTime gameTime)
        {
            // increment the elapsed time
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            // time for the next frame
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                if (Direction == Direction.Up || Direction == Direction.Down)
                {
                    currentFrameY = 100;
                    timeSinceLastFrame = 0; // reset elapsed time
                    frame++;
                    if (frame >= numFrames)
                    {
                        frame = 0;
                    }
                    currentFrameX = frameSizeX * frame;
                }

                if (Direction == Direction.Right || Direction == Direction.Left)
                {
                    currentFrameY = 0;
                    timeSinceLastFrame = 0; // reset elapsed time
                    frame++;
                    if (frame >= numFrames)
                    {
                        frame = 0;
                    }
                    currentFrameX = frameSizeX * frame;
                }
            }
        }

        /// <summary>
        /// Update Pacman Image
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {   
            //TO DO
            //get to pacmanPos coordinates
            Point location = new Point(0, 0);
            location = Point.MapToScreen(MapPos, new Point(5,5),new Point(500,500));
            pacmanPos.X = location.X;
            pacmanPos.Y = location.Y;

            if (Direction == Direction.Up)
            {
                spriteBatch.Draw(pacmanImg, pacmanPos, new Rectangle(currentFrameX, currentFrameY, frameSizeX, frameSizeY), Color.White);
            }
            if (Direction == Direction.Down)
            {
                spriteBatch.Draw(pacmanImg, pacmanPos, new Rectangle(currentFrameX, currentFrameY, frameSizeX, frameSizeY), Color.White, 
                                 0, Vector2.Zero, 1, SpriteEffects.FlipVertically, 0);
            }
            if (Direction == Direction.Left)
            {
                spriteBatch.Draw(pacmanImg, pacmanPos, new Rectangle(currentFrameX, currentFrameY, frameSizeX, frameSizeY), Color.White);
            }
            if (Direction == Direction.Right)
            {
                spriteBatch.Draw(pacmanImg, pacmanPos, new Rectangle(currentFrameX, currentFrameY, frameSizeX, frameSizeY), Color.White,
                                 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(pacmanImg, pacmanPos, new Rectangle(currentFrameX, currentFrameY, frameSizeX, frameSizeY), Color.White);
            }
        }

        /// <summary>
        /// Change direction based off of what key is down
        /// </summary>
        public override void GetNextDirection()
        {
            kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.W) == true) NextDirection = Direction.Up;
            if (kState.IsKeyDown(Keys.A) == true) NextDirection = Direction.Left;
            if (kState.IsKeyDown(Keys.S) == true) NextDirection = Direction.Down;
            if (kState.IsKeyDown(Keys.D) == true) NextDirection = Direction.Right;
        }
    }
}
