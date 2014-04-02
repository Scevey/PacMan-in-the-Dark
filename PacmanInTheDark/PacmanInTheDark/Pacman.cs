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


        /// <summary>
        /// Constructor to create Pacman Object
        /// </summary>
        /// <param name="myHealth">Starting health</param>
        /// <param name="myLives">Starting lives</param>
        /// <param name="path">Starting path</param>
        /// <param name="pos">Starting pos on path</param>
        public Pacman(Texture2D myPacman, Path path, float pos)
            : base(path, pos)
        {
            //Pacman starts with 100 health and 3 lives
            Health = 100;
            Lives = 3;
            
            //Starting image for pacman
            PacmanImg = myPacman;
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

        ///// <summary>
        ///// Update Pacman's image based off of corresponding direction and location on path
        ///// </summary>
        ///// <param name="direction">0-3</param>
        //public void Move(byte direction)
        //{
        //                
        //}

        //NOTE: Why do we need a draw method if we can just redraw accordingly in the Move method? 

        /// <summary>
        /// Update Pacman Image
        /// </summary>
        public override void Draw()
        {
        
        }

        public override Path GetNextPath()
        {
            return null;
        }
    }
}
