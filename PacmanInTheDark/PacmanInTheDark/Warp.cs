using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    class Warp : GamePiece
    {
        Warp warp2;
        public Warp(Path path, float pos)
            : base(path, pos)
        {        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Point topLeft, Point bottomRight)
        {
            
        }

        public override void Collision(Pacman pac)
        {
            pac.WarpCollision(warp2);
        }

        public override void Collision(Ghost ghost)
        {
            ghost.WarpCollision(warp2);
        }
    }
}
