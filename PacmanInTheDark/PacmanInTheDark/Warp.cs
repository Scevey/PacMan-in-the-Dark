using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    class Warp : GamePiece
    {
        //the warp this one links to
        Warp warp2;

        /// <summary>
        /// The public constructor. Makes both warps
        /// </summary>
        /// <param name="path">path of the first warp</param>
        /// <param name="pos">position of the first warp</param>
        /// <param name="path2">path of the second warp</param>
        /// <param name="pos2">position of the second warp</param>
        public Warp(Path path, float pos, Path path2, float pos2)
            : base(path, pos)
        {
            warp2 = new Warp(path2, pos2, this);
        }

        /// <summary>
        /// constructor for the second warp
        /// </summary>
        /// <param name="path">path for the second warp. Passed in from the other constructor</param>
        /// <param name="pos">position for the second warp. Passed in from the other constructor</param>
        /// <param name="_warp2">the first warp. Passed in from the other constructor</param>
        Warp(Path path, float pos, Warp _warp2) : base(path, pos)
        {
            warp2 = _warp2;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Point topLeft, Point bottomRight)
        {
            //this piece is not drawn
        }

        public override void Collision(Pacman pac)
        {
            if(!(pac.LastDirection == pac.CurrentDirection))
                pac.WarpCollision(warp2);

        }

        public override void Collision(Ghost ghost)
        {
            ghost.WarpCollision(warp2);
        }
    }
}
