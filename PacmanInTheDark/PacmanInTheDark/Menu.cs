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
    //Anthony Giallella
    //Sungmin Park
    class Menu
    {
        ///create a map object
        Map gameMap = new Map("map.txt");
        //Pacman object
        Pacman pacman;
        //Draw related attributes
        SpriteFont Font;
        Texture2D covered;

        //gamestates
        enum GameState { MainMenu, OptionMenu, InGame }
        
        //lists of gui for different states
        List<Gui> startMenu = new List<Gui>();
        List<Gui> optionMenu = new List<Gui>();
        List<Gui> inGame = new List<Gui>();
        GameState gameState;
        public Menu()
        {
            //add gui for images to lists based on states
            startMenu.Add(new Gui("menu"));
            startMenu.Add(new Gui("start"));
            startMenu.Add(new Gui("options"));
            
            optionMenu.Add(new Gui("optionsMenu"));
            optionMenu.Add(new Gui("back"));
            optionMenu.Add(new Gui("exit"));

            inGame.Add(new Gui("topBar"));
            //inGame.Add(new Gui("background"));    commented out to use a sample map 

        }
        public void LoadContent(ContentManager content)
        {
            //load in a pacmanImage and use it to create a pacman object
            Texture2D pacmanImage = content.Load<Texture2D>("PacManSheet");
            Font = content.Load<SpriteFont>("Arial");
            covered = content.Load<Texture2D>("blackCover");
            pacman = new Pacman(pacmanImage, gameMap.Paths[0], 0, .05f);

            //load, center and add click events for all in start list
            foreach (Gui gui in startMenu)
            {
                gui.LoadContent(content);
                gui.Center(600, 800);
                gui.clickEvent += OnClick;
            } 

            //adjust position
            startMenu.Find(x => x.ImgName == "start").MoveElement(0, 0);
            startMenu.Find(x => x.ImgName == "options").MoveElement(0, +125);
            
            //load, center and add click events for all in option list
            foreach (Gui gui in optionMenu)
            {
                gui.LoadContent(content);
                gui.Center(400, 400);
                gui.clickEvent += OnClick;
            }

            //adjust position
            optionMenu.Find(x => x.ImgName == "back").MoveElement(0, -50);
            optionMenu.Find(x => x.ImgName == "exit").MoveElement(0, 85);
            
            //load, center and add click events for all in ingame list
            foreach (Gui gui in inGame)
            {
                gui.LoadContent(content);
                gui.clickEvent += OnClick;
            }

            //adjust position
            //inGame.Find(x => x.ImgName == "background").Center(768, 768);      commented out to use a sample map
            //inGame.Find(x => x.ImgName == "background").MoveElement(0, 155);   commented out to use a sample map

        }
        public void Update(GameTime gameTime)
        {
            //change between game states
            switch (gameState)
            {
                case GameState.MainMenu:
                    //call gui to update and change
                    foreach (Gui gui in startMenu)
                    {
                        gui.Update();
                    }
                    break;
                case GameState.OptionMenu:
                    //call gui to update and change
                    foreach (Gui gui in optionMenu)
                    {
                        gui.Update();
                    }
                    break;
                case GameState.InGame:
                    //call gui to update and change, call pacman functionality
                    foreach (Gui gui in inGame)
                    {
                        gui.Update();
                        pacman.UpdateFrame(gameTime);
                        pacman.Move();
                    }
                    break;
                default:
                    break;
            }


        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //change between game states
            switch (gameState)
            {
                case GameState.MainMenu:
                    //change images based on state
                    foreach (Gui element in startMenu)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                case GameState.OptionMenu:
                    //change images based on state
                    foreach (Gui element in optionMenu)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                case GameState.InGame:
                    //change images based on state
                    foreach (Gui element in inGame)
                    {
                        element.Draw(spriteBatch);
                        //sample map that will later be replaced by the Gui "background"
                        //black horizontal Lines
                        spriteBatch.Draw(covered, new Vector2(0, 155), new Rectangle(0, 0, 600, 5), Color.White);
                        spriteBatch.Draw(covered, new Vector2(0, 255), new Rectangle(0, 0, 300, 5), Color.White);
                        spriteBatch.Draw(covered, new Vector2(400, 255), new Rectangle(0, 0, 200, 5), Color.White);
                        spriteBatch.Draw(covered, new Vector2(300, 750), new Rectangle(0, 0, 100, 5), Color.White);
                        //black vertical Lines
                        spriteBatch.Draw(covered, new Vector2(0, 155), new Rectangle(0, 0, 5, 100), Color.White);
                        spriteBatch.Draw(covered, new Vector2(300, 255), new Rectangle(0, 0, 5, 500), Color.White);
                        spriteBatch.Draw(covered, new Vector2(400, 255), new Rectangle(0, 0, 5, 500), Color.White);
                        spriteBatch.Draw(covered, new Vector2(595, 155), new Rectangle(0, 0, 5, 100), Color.White);
                    }
                    //info on topBar (will change later on to update the lives,score,pellets left, and hunger bar
                    //related to lives
                    spriteBatch.DrawString(Font, "Lives", new Vector2(42, 45), Color.White);
                    //related to score
                    spriteBatch.DrawString(Font, "Score", new Vector2(190, 45), Color.White);
                    spriteBatch.DrawString(Font, "0000000", new Vector2(175, 85), Color.White);
                    //related to pellets left
                    spriteBatch.DrawString(Font, "Left", new Vector2(390, 45), Color.White);
                    spriteBatch.DrawString(Font, " 00 ", new Vector2(390, 85), Color.White);
                    //related to hunger bar
                    spriteBatch.DrawString(Font, "Hunger", new Vector2(590, 45), Color.White);
                    spriteBatch.Draw(covered, new Vector2(536, 92), new Rectangle(0, 0, 200, 25), Color.White);

                    //draws pacman to the screen
                    pacman.Draw(gameTime, spriteBatch);
                    break;
                default:
                    break;
            }

        }
        //what buttons do when clicked
        public void OnClick(string element)
        {
            //name of image
            if (element == "start")
            {
                //what to do on click
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

            if (element == "exit")
            {
                Environment.Exit(0);
            }
        }
    }
}
