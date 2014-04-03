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
    class Gui
    {
        private Texture2D guiTexture;

        private Rectangle guiRectangle;

        private string imgName;

        public string ImgName
        {
            get { return imgName; }
            set { imgName = value; }
        }

        public delegate void ElementClicked(string element);

        public event ElementClicked clickEvent;

        public Gui(string imgName)
        {
            this.imgName = imgName;
        }

        public void LoadContent(ContentManager content)
        {
            guiTexture = content.Load<Texture2D>(imgName);
            guiRectangle = new Rectangle(0, 0, guiTexture.Width, guiTexture.Height);
        }

        public void Update()
        {
            if (guiRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)&& Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                clickEvent(imgName);
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(guiTexture, guiRectangle, Color.White);
        }

        public void Center(int height, int width)
        {
            guiRectangle = new Rectangle((width / 2) - (this.guiTexture.Width / 2), (height / 2) - (this.guiTexture.Height / 2), this.guiTexture.Width, this.guiTexture.Height);
        }

        public void MoveElement(int x, int y)
        {
            guiRectangle = new Rectangle(guiRectangle.X += x, guiRectangle.Y += y, guiRectangle.Width, guiRectangle.Height);
        }
    }
}
