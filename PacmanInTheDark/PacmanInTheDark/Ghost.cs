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
    class Ghost : MovableGamePiece
    {
        //Random variable used to decide which path/direction ghost has
        Random randy;

        //Ghost image
        Texture2D ghostImg;
        public Texture2D GhostImg
        {
            get { return ghostImg; }
            set { ghostImg = value; }
        }

        /// <summary>
        /// Constructor to create Ghost
        /// </summary>
        /// <param name="myGhostImg">Starting image</param>
        /// <param name="path">Starting path</param>
        /// <param name="pos">Starting pos. on path</param>
        public Ghost(Texture2D myGhostImg, Path path, float pos)
            : base(path, pos)
        {
            //Starting image for ghost
            GhostImg = myGhostImg;
        }

        /// <summary>
        /// Update Ghost's image based off of corresponding direction and location on path
        /// </summary>
        /// <param name="direction">0-3</param>
        public void Move(byte direction)
        {
            bool isEven = ((PathPos % 2) == 0);
            //TODO
            //Add textures for each direction and change whether pos. is even/odd (two different images to give animation)
            //Change path accordingly
            switch (direction)
            {
                case 0: //Up
                    //if (isEven) 
                    //{
                    //    GhostImg =   
                    //    PathChange(...)
                    //}
                    //else GhostImg = 
                    break;
                case 1: //Left
                    //if (isEven) 
                    //{
                    //    GhostImg =   
                    //    PathChange(...)
                    //}
                    //else GhostImg = 
                    break;
                case 2: //Down
                    //if (isEven) 
                    //{
                    //    GhostImg =   
                    //    PathChange(...)
                    //}
                    //else GhostImg = 
                    break;
                case 3: //Right
                    //if (isEven) 
                    //{
                    //    GhostImg =   
                    //    PathChange(...)
                    //}
                    //else GhostImg = 
                    break;
            }
        }

        public override void Draw()
        {

        }

        public override Path GetNextPath()
        {
            return null;
        }
    }
}
