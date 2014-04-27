using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    class Pellet : GamePiece
    {
        bool active;
        bool Active
        {
            get
            {
                return active;
            }
        }
        public Pellet(Path p, float pos)
            : base(p,pos)
        {

        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Point topLeft, Point bottomRight)
        {
            throw new NotImplementedException();
        }

        public override void PacmanCollision(Pacman pac)
        {
            throw new NotImplementedException();
        }
    }
}
