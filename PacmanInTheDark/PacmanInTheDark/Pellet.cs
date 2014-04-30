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
    class Pellet : GamePiece
    {
        bool active = true;
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

        const int yPosOffSet = 152; // how off pacman's y coordinate is from the map
        const int xPosOffSet = -10; // how off pacman's x coordinate is from the map
        Vector2 pelletPos; // position of pellet

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Point mapCoord, Point pixelCoord)
        {
            //leave this line in
            if (!active) return;

            //add code here

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Point MapCoord, Point PixelCoord , Texture2D image)
        {
            //get to pelletPos coordinates
            Point location = new Point(0, 0);

            // convert path location to screen location
            location = Point.MapToScreen(MapPos, MapCoord, PixelCoord);

            // store converted points into a Vector2D
            pelletPos.X = location.X + xPosOffSet;
            pelletPos.Y = location.Y + yPosOffSet;

            spriteBatch.Draw(image, new Rectangle((int)pelletPos.X, (int)pelletPos.Y, 50, 50), Color.White);
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
