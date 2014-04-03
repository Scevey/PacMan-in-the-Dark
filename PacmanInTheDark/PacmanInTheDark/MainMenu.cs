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
        enum GameState { MainMenu, OptionMenu, InGame }
        List<Gui> startMenu = new List<Gui>();
        List<Gui> optionMenu = new List<Gui>();
        List<Gui> inGame = new List<Gui>();
        GameState gameState;
        public MainMenu()
        {
            startMenu.Add(new Gui("menu"));
            startMenu.Add(new Gui("start"));
            startMenu.Add(new Gui("options"));

            optionMenu.Add(new Gui("optionsMenu"));
            optionMenu.Add(new Gui("back"));
            optionMenu.Add(new Gui("exit"));
            optionMenu.Add(new Gui("tool"));

            inGame.Add(new Gui("background"));

        }
        public void LoadContent(ContentManager content)
        {
            foreach (Gui gui in startMenu)
            {
                gui.LoadContent(content);
                gui.Center(600, 800);
                gui.clickEvent += OnClick;
            } 

            startMenu.Find(x => x.ImgName == "start").MoveElement(0, 0);
            startMenu.Find(x => x.ImgName == "options").MoveElement(0, +125);
            
            foreach (Gui gui in optionMenu)
            {
                gui.LoadContent(content);
                gui.Center(400, 400);
                gui.clickEvent += OnClick;
            }

            optionMenu.Find(x => x.ImgName == "back").MoveElement(0, -100);
            optionMenu.Find(x => x.ImgName == "tool").MoveElement(0, 15);
            optionMenu.Find(x => x.ImgName == "exit").MoveElement(0, 135);

            foreach (Gui gui in inGame)
            {
                gui.LoadContent(content);
                gui.Center(768, 768);
                gui.clickEvent += OnClick;
            }
        }
        public void Update()
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    foreach (Gui gui in startMenu)
                    {
                        gui.Update();
                    }
                    break;
                case GameState.OptionMenu:
                    foreach (Gui gui in optionMenu)
                    {
                        gui.Update();
                    }
                    break;
                case GameState.InGame:
                    foreach (Gui gui in inGame)
                    {
                        gui.Update();
                    }
                    break;
                default:
                    break;
            }


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    foreach (Gui element in startMenu)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                case GameState.OptionMenu:
                    foreach (Gui element in optionMenu)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                case GameState.InGame:
                    foreach (Gui element in inGame)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                default:
                    break;
            }

        }
        //what buttons do when clicked
        public void OnClick(string element)
        {
            if (element == "start")
            {
                gameState = GameState.InGame;
            }
            if (element == "options")
            {
                gameState = GameState.OptionMenu;
            }
            if (element == "back")
            {
                gameState = GameState.MainMenu;
            }
            if (element == "tool")
            {
              //call variable editor
            }
            if (element == "exit")
            {
                Environment.Exit(0);
            }
        }
    }
}
