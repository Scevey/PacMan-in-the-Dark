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
    class MainMenu
    {
        List<Gui> startMenu = new List<Gui>();

        public MainMenu()
        {
            startMenu.Add(new Gui("Menu"));
            startMenu.Add(new Gui("Start"));
            startMenu.Add(new Gui("Options"));
        }
        public void LoadContent(ContentManager content)
        {
            foreach (Gui gui in startMenu)
            {
                gui.LoadContent(content);
                gui.Center(600, 800);
            }
            startMenu.Find(x => x.ImgName == "Start").MoveElement(0, -100);
            startMenu.Find(x => x.ImgName == "Options").MoveElement(0, +100);
        }
        public void Update()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Gui element in startMenu)
            {
                element.Draw(spriteBatch);
            }
        }
    }
}
