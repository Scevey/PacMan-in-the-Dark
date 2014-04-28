using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    class Pellet : GamePiece
    {
        bool active;
        public bool Active
        {
            get
            {
                return active;
            }
        }
        public Pellet(Path p, float pos)
            : base(p,pos)
        {
            this.active = true;
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Point topLeft, Point bottomRight)
        {
            //leave this line in
            if (!active) return;

            //add code here
        }

        public override void Collision(Pacman pac)
        {
            //leave this line
            if (!active) return;

            // collides with pacman - becomes inactive
            this.active = false;

            //add code here
        }

        public override void Collision(Ghost ghost)
        {
        }
    }
}
