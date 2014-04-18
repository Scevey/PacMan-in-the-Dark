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
        //Random variable used to decide which path/direction ghost has
        Random randy = new Random();

        //Ghost image
        Texture2D ghostImg;
        public Texture2D GhostImg
        {
            get { return ghostImg; }
            set { ghostImg = value; }
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

        /// <summary>
        /// Constructor to create Ghost
        /// </summary>
        /// <param name="myGhostImg">Starting image</param>
        /// <param name="path">Starting path</param>
        /// <param name="pos">Starting pos. on path</param>
        public Ghost(Texture2D myGhostImg, Path path, float pos, float speed)
            : base(path, pos, speed)
        {
            //Starting information for ghost
            GhostImg = myGhostImg;
            originalSpeed = speed;
            originalPos = pos;
            originalPath = path;
        }

        /// <summary>
        /// Update Ghost's image based off of corresponding direction and location on path
        /// </summary>
        /// <param name="direction">0-3</param>
        public void Move(byte direction)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Pick ghost's next direction (RANDOM GHOST)
        /// </summary>
        public override void GetNextDirection()
        {
            // Note: Do we want to have ghost be able to not have a not direction? AKA nextDirection = Direction.None?
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
    }
}
