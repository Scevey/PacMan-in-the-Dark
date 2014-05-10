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
using System.IO;
using System.Diagnostics;

namespace PacmanInTheDark
{
    //Anthony Giallella
    //Sungmin Park
    //Jeremy Hall

    //handles the creation of menus and manage gamestates
    class Menu
    {

        //create a map object
        Map gameMap = new Map("map.txt");
        Pacman pacman;//Pacman object
        Ghost Blinky; // Red Ghost
        Ghost Pinky; // Pink ghost
        Ghost Inky; // Blue Ghost
        Ghost Clyde; // Orange Ghost

        //Draw related attributes
        SpriteFont Font;

        Texture2D pacmanImage; // pacmans image
        Texture2D vision; // pacmans sight image
        Texture2D pelletImg; // pellet image
        Texture2D bg; // path image

        Path currentPath;
        GraphicsDevice gd;
        //gameplay window dimensions
        const int maxWidth = 800;
        const int maxHeight = 600;
        //gamestates
        enum GameState { MainMenu, OptionMenu, Info, InGame, Pause, EndGame, WinGame, HighScores }

        // attributes for starting values of the game
        float speed; // how fast pacman travels
        int ghosts; // how many ghosts the game starts with
        int lives; // how many lives pacman starts with
        float health; // how much health pacman has
        int hunger; // how fast the bar drains
        int light; // the level of light

        //window sizes

        // hi score attributes
        Scores hiScores;

        //lists of gui for different states
        List<Gui> startMenu = new List<Gui>();
        List<Gui> optionMenu = new List<Gui>();
        List<Gui> infoMenu = new List<Gui>();
        List<Gui> inGame = new List<Gui>();
        List<Gui> pauseMenu = new List<Gui>();
        List<Gui> end = new List<Gui>();
        List<Gui> win = new List<Gui>();
        List<Gui> highScore = new List<Gui>();

        GameState gameState;

        //add gui items to each list
        public Menu(GraphicsDevice _gd)
        {
            //add gui for images to lists based on states
            //main menu images
            startMenu.Add(new Gui("Start Menu Base"));
            startMenu.Add(new Gui("Start Button"));
            startMenu.Add(new Gui("Option"));

            //option menu images
            optionMenu.Add(new Gui("Option Base"));
            optionMenu.Add(new Gui("Back Button"));
            optionMenu.Add(new Gui("Exit Button"));
            optionMenu.Add(new Gui("Info Button"));

            //game description images
            infoMenu.Add(new Gui("Info"));
            infoMenu.Add(new Gui("Info Back"));

            inGame.Add(new Gui("topBar"));
            //inGame.Add(new Gui("background"));    commented out to use a sample map 

            //pause menu images
            pauseMenu.Add(new Gui("Pause Base"));
            pauseMenu.Add(new Gui("Exit Button"));
            pauseMenu.Add(new Gui("Back Button2"));

            //game over images
            end.Add(new Gui("End Screen Base"));
            end.Add(new Gui("HighScore Button"));

            //game won images
            win.Add(new Gui("Win Screen Base"));
            win.Add(new Gui("HighScore Button"));
            //win.Add(new Gui("Continue Button"));

            //post game images
            highScore.Add(new Gui("HighScore"));
            highScore.Add(new Gui("HighScore Exit"));
            highScore.Add(new Gui("HighScore Main"));

            //graphics device
            gd = _gd;

        }

        /// <summary>
        /// Loads all required starting content for game
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            // Loads values from editor file
            LoadEditorValues();

            // Create high scores object
            hiScores = new Scores(); 

            // Load various images
            pacmanImage = content.Load<Texture2D>("PacManSheet"); // load pacman sprite sheet
            Texture2D ghostImage = content.Load<Texture2D>("ghostSheet"); // load ghost sprite sheet
            Texture2D glowImage = content.Load<Texture2D>("glowSheet"); // load ghost glow sheet
            pelletImg = content.Load<Texture2D>("Pellet"); // load pellet image
            vision = content.Load<Texture2D>("Vision"); // load pacman sight image
            Font = content.Load<SpriteFont>("Arial");

            // Create pacman and set initial info
            pacman = new Pacman(pacmanImage, vision, gameMap.Paths[6], 7.5f, speed);
            pacman.Health = health;
            pacman.Lives = lives;
            pacman.Light = light;

            // Create the specified amount of ghosts from the editor
            #region
            switch (ghosts)
            {
                case 0: // Should never be 0, but in case -- just create all
                    Clyde = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 2f, .1f);
                    Blinky = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 0, .1f);
                    Pinky = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 0, .1f);
                    Inky = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 0, .1f);
                    break;
                case 1: // Creates one ghost
                    Clyde = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 2f, .1f);
                    break;
                case 2: // Creates two ghosts
                    Clyde = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 2f, .1f);
                    Blinky = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 0, .1f);
                    break;
                case 3: // Creates three ghosts
                    Clyde = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 2f, .1f);
                    Blinky = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 0, .1f);
                    Pinky = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 0, .1f);
                    break;
                case 4: // Creates four ghosts
                    Clyde = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 2f, .1f);
                    Blinky = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 0, .1f);
                    Pinky = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 0, .1f);
                    Inky = new Ghost(ghostImage, glowImage, gameMap.Paths[12], 0, .1f);
                    break;
            }
            #endregion

            // Set the starting path and create the map
            currentPath = (pacman.CurrentPath);
            bg = Map.DrawMap(gameMap, gd);

            //load, center and add click events for all in start list
            foreach (Gui gui in startMenu)
            {
                gui.LoadContent(content);
                gui.Center(780, 1340);
                gui.clickEvent += OnClick;
            }

            //adjust position
            startMenu.Find(x => x.ImgName == "Start Button").MoveElement(-135, +200);
            startMenu.Find(x => x.ImgName == "Option").MoveElement(115, 200);

            //load, center and add click events for all in option list
            foreach (Gui gui in optionMenu)
            {
                gui.LoadContent(content);
                gui.Center(780, 1340);
                gui.clickEvent += OnClick;
            }

            //adjust position
            optionMenu.Find(x => x.ImgName == "Back Button").MoveElement(0, 225);
            optionMenu.Find(x => x.ImgName == "Exit Button").MoveElement(0, 40);
            optionMenu.Find(x => x.ImgName == "Info Button").MoveElement(0, -150);

            foreach (Gui gui in infoMenu)
            {
                gui.LoadContent(content);
                gui.Center(780, 1340);
                gui.clickEvent += OnClick;
            }

            //adjust position
            infoMenu.Find(x => x.ImgName == "Info Back").MoveElement(0, 225);

            //load, center and add click events for all in ingame list
            foreach (Gui gui in inGame)
            {
                gui.LoadContent(content);
                gui.clickEvent += OnClick;
            }

            foreach (Gui gui in pauseMenu)
            {
                gui.LoadContent(content);
                gui.Center(780, 1340);
                gui.clickEvent += OnClick;
            }

            //adjust position
            pauseMenu.Find(x => x.ImgName == "Back Button2").MoveElement(0, -50);
            pauseMenu.Find(x => x.ImgName == "Exit Button").MoveElement(0, 115);

            foreach (Gui gui in end)
            {
                gui.LoadContent(content);
                gui.Center(780, 1340);
                gui.clickEvent += OnClick;
            }
            end.Find(x => x.ImgName == "HighScore Button").MoveElement(0, 25);

            foreach (Gui gui in win)
            {
                gui.LoadContent(content);
                gui.Center(780, 1340);
                gui.clickEvent += OnClick;
            }
            win.Find(x => x.ImgName == "HighScore Button").MoveElement(0, 25);

            foreach (Gui gui in highScore)
            {
                gui.LoadContent(content);
                gui.Center(780, 1340);
                gui.clickEvent += OnClick;
            }

            //adjust position
            highScore.Find(x => x.ImgName == "HighScore Main").MoveElement(-125, 225);
            highScore.Find(x => x.ImgName == "HighScore Exit").MoveElement(125, 225);
        }

        /// <summary>
        /// Game loop
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //change between game states
            switch (gameState)
            {
                // Update MainMenu
                #region
                case GameState.MainMenu:
                    //call gui to update and change
                    foreach (Gui gui in startMenu)
                    {
                        gui.Update();
                    }
                    break;
                #endregion

                // Update OptionMenu
                #region
                case GameState.OptionMenu:
                    //call gui to update and change
                    foreach (Gui gui in optionMenu)
                    {
                        gui.Update();
                    }
                    break;
                #endregion

                // Update Info menu
                #region
                case GameState.Info:
                    foreach (Gui gui in infoMenu)
                    {
                        gui.Update();
                    }
                    break;
                #endregion
                
                // Update InGame menu
                #region
                case GameState.InGame:
                    //call gui to update and change, call pacman functionality
                    foreach (Gui gui in inGame)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            gameState = GameState.Pause;
                        }
                        if (pacman.isGameOver() == true)
                        {
                            gameState = GameState.EndGame;
                        }
                        gui.Update();

                        // Move objects (ghost(s), pacman)
                        pacman.UpdateFrame(gameTime);
                        pacman.Move();
                        pacman.LoseHealth(.4f, gameTime);
                        pacman.NoHealth();
                        // Updates ghosts in real-time
                        #region
                        switch (ghosts)
                        {
                            case 0: // Should never be 0, but in case -- just create all
                                Clyde.UpdateFrame(gameTime);
                                Clyde.UpdateFrame(gameTime);
                                Blinky.UpdateFrame(gameTime);
                                Pinky.UpdateFrame(gameTime);
                                Inky.UpdateFrame(gameTime);
                                Clyde.Move();
                                Blinky.Move();
                                Pinky.Move();
                                Inky.Move();
                                break;
                            case 1: // Creates one ghost
                                Clyde.UpdateFrame(gameTime);
                                Clyde.Move();
                                break;
                            case 2: // Creates two ghosts
                                Clyde.UpdateFrame(gameTime);
                                Blinky.UpdateFrame(gameTime);
                                Clyde.Move();
                                Blinky.Move();
                                break;
                            case 3: // Creates three ghosts
                                Clyde.UpdateFrame(gameTime);
                                Blinky.UpdateFrame(gameTime);
                                Pinky.UpdateFrame(gameTime);
                                Clyde.Move();
                                Blinky.Move();
                                Pinky.Move();
                                break;
                            case 4: // Creates four ghosts
                                Clyde.UpdateFrame(gameTime);
                                Clyde.UpdateFrame(gameTime);
                                Blinky.UpdateFrame(gameTime);
                                Pinky.UpdateFrame(gameTime);
                                Inky.UpdateFrame(gameTime);
                                Clyde.Move();
                                Blinky.Move();
                                Pinky.Move();
                                Inky.Move();
                                break;
                        }
                        #endregion
                       
                        //check for collisions with pacman in pacman's current path
                        List<GamePiece> collisionList = new List<GamePiece>();
                        //check for collisions with pacman in pacman's current path
                        foreach (GamePiece gp in pacman.CurrentPath.pieces)
                        {
                            collisionList.Add(gp);
                        }
                        foreach (GamePiece gp in collisionList)
                        {
                            if (gp == pacman)
                                continue;
                            if (Point.Distance(gp.MapPos, pacman.MapPos) <= pacman.Speed)
                                gp.Collision(pacman);
                        }
                        currentPath = (pacman.CurrentPath);
                    }
                    break;
                #endregion

                //update pause menu
                #region
                case GameState.Pause:
                    foreach (Gui gui in pauseMenu)
                    {
                        gui.Update();
                    }
                    break;
                //update end game screen
                case GameState.EndGame:
                    foreach (Gui gui in end)
                    {
                        gui.Update();
                    }
                    break;
                #endregion

                //update win game screen
                #region
                case GameState.WinGame:
                    foreach (Gui gui in win)
                    {
                        gui.Update();
                    }
                    break;
                #endregion

                //update highscores page
                #region
                case GameState.HighScores:
                    foreach (Gui gui in highScore)
                    {
                        gui.Update();
                    }
                    break;
                #endregion
                default:
                    break;
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int windowWidth;
            int windowHeight;
            float screenRatio = maxWidth / maxHeight;
            //change between game states
            switch (gameState)
            {
                // Draw Main Menu
                #region
                case GameState.MainMenu:
                    //draws start screen
                    foreach (Gui element in startMenu)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                #endregion

                // Draw Option Menu
                #region
                case GameState.OptionMenu:
                    //draws option menu
                    foreach (Gui element in optionMenu)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                #endregion

                // Draw Info Menu
                #region
                case GameState.Info:
                    //draws game info
                    foreach (Gui element in infoMenu)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                #endregion

                // Draw InGame
                #region
                case GameState.InGame:
                    //draws gameplay
                    //original values 1230, 530

                    if (gameMap.MapRatio > screenRatio)
                    {
                        windowWidth = maxWidth;
                        windowHeight = (int)(maxHeight / gameMap.MapRatio);
                    }
                    else if (gameMap.MapRatio == screenRatio)
                    {
                        windowWidth = maxWidth;
                        windowHeight = maxHeight;
                    }
                    else
                    {
                        windowWidth = (int)(maxWidth * gameMap.MapRatio);
                        windowHeight = maxHeight;
                    }

                    spriteBatch.Draw(bg, new Rectangle(0, 170, windowWidth, windowHeight), Color.White);

                    // Check each pellet to see if it's active and if it is, draw it
                    for (int i = 0; i < gameMap.Pellets.Count; i++)
                    {
                        if (gameMap.Pellets[i].Active == true)
                        {
                            gameMap.Pellets[i].Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), pelletImg);
                        }
                    }

                    // Change gamestate to win b/c there are no more pellets (Change to > 0 for testing high scores)
                    if (gameMap.PelletCount == 0) gameState = GameState.WinGame;

                    // Ghost Drawing
                    #region
                    switch (ghosts)
                    {
                        case 0: // Should never be 0, but in case -- just create all
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            Inky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 300);
                            break;
                        case 1: // Creates one ghost
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);

                            break;
                        case 2: // Creates two ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            break;
                        case 3: // Creates three ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            break;
                        case 4: // Creates four ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            Inky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 300);
                            break;
                    }
                    #endregion

                    //draws pacman to the screen
                    pacman.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight));
                    foreach (Gui element in inGame)
                    {
                        element.Draw(spriteBatch);
                    }

                    //info on topBar (will change later on to update the lives,score,pellets left, and hunger bar
                    //related to lives
                    spriteBatch.DrawString(Font, "Lives", new Vector2(42, 45), Color.White);
                    for (int i = 0; i < pacman.Lives; i++)
                    {
                        spriteBatch.Draw(pacmanImage, new Rectangle(10 + (i * 25), 80, 30, 30), new Rectangle(0, 0, 100, 100), Color.White);
                    }
                    // Drawing Status Bar
                    #region
                    //related to score
                    spriteBatch.DrawString(Font, "Score", new Vector2(190, 45), Color.White);
                    spriteBatch.DrawString(Font, Convert.ToString(pacman.Score), new Vector2(185, 85), Color.White);

                    //related to pellets left
                    spriteBatch.DrawString(Font, "Left", new Vector2(390, 45), Color.White);
                    spriteBatch.DrawString(Font, Convert.ToString(gameMap.PelletCount), new Vector2(390, 85), Color.White);

                    //related to hunger bar
                    spriteBatch.DrawString(Font, "Hunger", new Vector2(590, 45), Color.White);
                    spriteBatch.Draw(vision, new Vector2(536, 92), new Rectangle(0, 0, (int)(200 * (pacman.Health / 100)), 25), Color.White);
                    #endregion
                    break;
                    #endregion

                // Draw Pause Menu
                #region
                case GameState.Pause:
                    if (gameMap.MapRatio > screenRatio)
                    {
                        windowWidth = maxWidth;
                        windowHeight = (int)(maxHeight / gameMap.MapRatio);
                    }
                    else if (gameMap.MapRatio == screenRatio)
                    {
                        windowWidth = maxWidth;
                        windowHeight = maxHeight;
                    }
                    else
                    {
                        windowWidth = (int)(maxWidth * gameMap.MapRatio);
                        windowHeight = maxHeight;
                    }

                    // Map texture
                    spriteBatch.Draw(bg, new Rectangle(0, 170, windowWidth, windowHeight), Color.White);

                    // Ghosts drawing
                    #region
                    switch (ghosts)
                    {
                        case 0: // Should never be 0, but in case -- just create all
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            Inky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 300);
                            break;
                        case 1: // Creates one ghost
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);

                            break;
                        case 2: // Creates two ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            break;
                        case 3: // Creates three ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            break;
                        case 4: // Creates four ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            Inky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 300);
                            break;
                    }
                    #endregion

                    // Draws pellets
                    for (int i = 0; i < gameMap.Pellets.Count; i++)
                    {
                        if (gameMap.Pellets[i].Active == true)
                        {
                            gameMap.Pellets[i].Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), pelletImg);
                        }
                    }
                    // Draw pacman
                    pacman.Draw(gameTime, spriteBatch, new Point(28, 26), new Point(1180, 500));
                    foreach (Gui element in inGame)
                    {
                        element.Draw(spriteBatch);
                    }
                    //related to score
                    spriteBatch.DrawString(Font, "Score", new Vector2(190, 45), Color.White);
                    spriteBatch.DrawString(Font, Convert.ToString(pacman.Score), new Vector2(185, 85), Color.White);

                    //related to pellets left
                    spriteBatch.DrawString(Font, "Left", new Vector2(390, 45), Color.White);
                    spriteBatch.DrawString(Font, Convert.ToString(gameMap.PelletCount), new Vector2(390, 85), Color.White);

                    //related to hunger bar
                    spriteBatch.DrawString(Font, "Hunger", new Vector2(590, 45), Color.White);
                    spriteBatch.Draw(vision, new Vector2(536, 92), new Rectangle(0, 0, 200, 25), Color.White);

                    foreach (Gui element in pauseMenu)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                #endregion

                // Draw the end game screen with game behind it
                #region
                case GameState.EndGame:
                    if (gameMap.MapRatio > screenRatio)
                    {
                        windowWidth = maxWidth;
                        windowHeight = (int)(maxHeight / gameMap.MapRatio);
                    }
                    else if (gameMap.MapRatio == screenRatio)
                    {
                        windowWidth = maxWidth;
                        windowHeight = maxHeight;
                    }
                    else
                    {
                        windowWidth = (int)(maxWidth * gameMap.MapRatio);
                        windowHeight = maxHeight;
                    }

                    spriteBatch.Draw(bg, new Rectangle(0, 170, windowWidth, windowHeight), Color.White);

                    // Draw ghosts
                    #region
                    switch (ghosts)
                    {
                        case 0: // Should never be 0, but in case -- just create all
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            Inky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 300);
                            break;
                        case 1: // Creates one ghost
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);

                            break;
                        case 2: // Creates two ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            break;
                        case 3: // Creates three ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            break;
                        case 4: // Creates four ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            Inky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 300);
                            break;
                    }
                    #endregion

                    // Draw pellets
                    for (int i = 0; i < gameMap.Pellets.Count; i++)
                    {
                        if (gameMap.Pellets[i].Active == true)
                        {
                            gameMap.Pellets[i].Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), pelletImg);
                        }
                    }
                    foreach (Gui element in inGame)
                    {
                        element.Draw(spriteBatch);
                    }
                    spriteBatch.Draw(bg, new Rectangle(0, 170, 1230, 530), Color.White);
                    //related to score
                    spriteBatch.DrawString(Font, "Score", new Vector2(190, 45), Color.White);
                    spriteBatch.DrawString(Font, Convert.ToString(pacman.Score), new Vector2(185, 85), Color.White);

                    //related to pellets left
                    spriteBatch.DrawString(Font, "Left", new Vector2(390, 45), Color.White);
                    spriteBatch.DrawString(Font, Convert.ToString(gameMap.PelletCount), new Vector2(390, 85), Color.White);

                    //related to hunger bar
                    spriteBatch.DrawString(Font, "Hunger", new Vector2(590, 45), Color.White);
                    spriteBatch.Draw(vision, new Vector2(536, 92), new Rectangle(0, 0, 200, 25), Color.White);
                    // draw light level
                    spriteBatch.Draw(vision, new Vector2(536, 92), new Rectangle(0, 0, 200, 25), Color.White);
                    foreach (Gui element in end)
                    {
                        element.Draw(spriteBatch);
                    }

                    // Load the hiScores -- use the property to access each individual score
                    hiScores.LoadScores();
                    // Check if user got a high score
                    if (hiScores.HiScores.Count == 9) // The case that the hiscores list is full
                    {
                        // Needs to beat at least the lowest high score to make it
                        if (pacman.Score > hiScores.HiScores[9].Score) // User made it
                        {
                            // Get user input for high score


                            // Write the score to the file
                            hiScores.WriteScores("", 0);
                        }
                    }
                    else if (hiScores.HiScores.Count < 9) // The case that the hiscores list is not full
                    {
                        // User automatically makes it to high scores

                        // Get user input for high score

                        // Write the score to the file
                        hiScores.WriteScores("", 0);
                    }
                    break;
                #endregion

                // Draw WinGame State
                #region
                case GameState.WinGame:
                    if (gameMap.MapRatio > screenRatio)
                    {
                        windowWidth = maxWidth;
                        windowHeight = (int)(maxHeight / gameMap.MapRatio);
                    }
                    else if (gameMap.MapRatio == screenRatio)
                    {
                        windowWidth = maxWidth;
                        windowHeight = maxHeight;
                    }
                    else
                    {
                        windowWidth = (int)(maxWidth * gameMap.MapRatio);
                        windowHeight = maxHeight;
                    }

                    spriteBatch.Draw(bg, new Rectangle(0, 170, windowWidth, windowHeight), Color.White);

                    #region
                    switch (ghosts)
                    {
                        case 0: // Should never be 0, but in case -- just create all
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            Inky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 300);
                            break;
                        case 1: // Creates one ghost
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);

                            break;
                        case 2: // Creates two ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            break;
                        case 3: // Creates three ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            break;
                        case 4: // Creates four ghosts
                            Clyde.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 0);
                            Blinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 100);
                            Pinky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 200);
                            Inky.Draw(gameTime, spriteBatch, gameMap.MapSize, new Point(windowWidth, windowHeight), 300);
                            break;
                    }
                    #endregion

                    foreach (Gui element in inGame)
                    {
                        element.Draw(spriteBatch);
                    }
                    spriteBatch.Draw(bg, new Rectangle(0, 170, 1230, 530), Color.White);
                    //related to score
                    spriteBatch.DrawString(Font, "Score", new Vector2(190, 45), Color.White);
                    spriteBatch.DrawString(Font, Convert.ToString(pacman.Score), new Vector2(185, 85), Color.White);

                    //related to pellets left
                    spriteBatch.DrawString(Font, "Left", new Vector2(390, 45), Color.White);
                    spriteBatch.DrawString(Font, Convert.ToString(gameMap.PelletCount), new Vector2(390, 85), Color.White);

                    //related to hunger bar
                    spriteBatch.DrawString(Font, "Hunger", new Vector2(590, 45), Color.White);
                    spriteBatch.Draw(vision, new Vector2(536, 92), new Rectangle(0, 0, 200, 25), Color.White);
                    // draw light level
                    spriteBatch.Draw(vision, new Vector2(536, 92), new Rectangle(0, 0, 200, 25), Color.White);
                    foreach (Gui element in win)
                    {
                        element.Draw(spriteBatch);
                    }

                    // Load the hiScores -- use the property to access each individual score
                    hiScores.LoadScores();
                    // Check if user got a high score
                    if (hiScores.HiScores.Count == 9) // The case that the hiscores list is full
                    {
                        // Needs to beat at least the lowest high score to make it
                        if (pacman.Score > hiScores.HiScores[9].Score) // User made it
                        {
                            // Get user input for high score


                            // Write the score to the file
                            hiScores.WriteScores("", 0);
                        }
                    }
                    else if (hiScores.HiScores.Count < 9) // The case that the hiscores list is not full
                    {
                        // User automatically makes it to high scores

                        // Get user input for high score

                        // Write the score to the file
                        hiScores.WriteScores("", 0);
                    }
                    break;
                #endregion

                // Draw Highscores Menu
                #region
                case GameState.HighScores:
                    foreach (Gui element in highScore)
                    {
                        element.Draw(spriteBatch);
                    }
                   
                    // Load the hiScores -- use the property to access each individual score
                    hiScores.LoadScores();

                    // Draws each hi-score
                    int x = 500; // First x spot
                    int y = 210; // First y spot
                    for (int i = 0; i < hiScores.HiScores.Count; i++)
                    {
                        // Needs to move over to otherside of high scores
                        if (i == 5)
                        {
                            x += 235;
                            y = 210;
                        }
                        spriteBatch.DrawString(Font, hiScores.HiScores[i].Name + " - " + hiScores.HiScores[i].Score,
                                                new Vector2(x, y), Color.Yellow);
                        y += 75;
                    }


                    break;
                #endregion
                default:
                    break;
            }

        }
        //what buttons do when clicked
        public void OnClick(string element)
        {
            //name of image
            if (element == "Start Button")
            {
                //what to do on click
                gameState = GameState.InGame;
            }
            if (element == "Option")
            {
                gameState = GameState.OptionMenu;
            }
            if (element == "Back Button")
            {
                gameState = GameState.MainMenu;
            }
            if (element == "Back Button2")
            {
                gameState = GameState.InGame;
            }
            if (element == "Info Button")
            {
                gameState = GameState.Info;
            }
            if (element == "Info Back")
            {
                gameState = GameState.OptionMenu;
            }
            if (element == "Exit Button")
            {
                Environment.Exit(0);
            }
            if (element == "HighScore Exit")
            {
                Environment.Exit(0);
            }
            if (element == "HighScore Button")
            {
                gameState = GameState.HighScores;
            }
            if (element == "HighScore Main")
            {
                gameState = GameState.MainMenu;
            }
        }

        /// <summary>
        /// Loads in all of the values from the editor
        /// </summary>
        public void LoadEditorValues()
        {
            try
            {
                // Creates reader to read in editor values
                StreamReader reader = new StreamReader("editor.txt");

                // Set attributes
                speed = (float.Parse(reader.ReadLine()) / 10);
                ghosts = int.Parse(reader.ReadLine());
                lives = int.Parse(reader.ReadLine());
                health = float.Parse(reader.ReadLine());
                hunger = int.Parse(reader.ReadLine());
                light = int.Parse(reader.ReadLine());

                reader.Close();
            }
            //set values if fnf
            catch (FileNotFoundException)
            {
                speed = 1;
                ghosts = 3;
                lives = 3;
                health = 100;
                hunger = 1;
                light = 3;
            }
            //other exceptions
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
