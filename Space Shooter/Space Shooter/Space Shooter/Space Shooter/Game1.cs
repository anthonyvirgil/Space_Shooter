using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Space_Shooter
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();

        public int enemyBulletDamage;
        public Texture2D menuImage;
        public Texture2D gameoverImage;

        // State Enum
        public enum State
        {
            Menu,
            Playing,
            Gameover
        }

        // Lists
        List<Asteroid> asteroidList = new List<Asteroid>();
        List<Enemy> enemyList = new List<Enemy>();
        List<Explosion> explosionList = new List<Explosion>();
        List<string> menuItems;

        // Instantiate objects
        private Player player = new Player();
        private StarField starField = new StarField();
        private HUD hud = new HUD();
        private SoundManager sm = new SoundManager();
        private SpriteFont mainMenuItemsFont;
        private SpriteFont mainMenuTitleFont;
        private SpriteFont gameoverMenuItemFont;
        private int selected = 0;
        private Color color;
        private int screenHeight;
        private int screenWidth;
        private int padding; 
        private KeyboardState keyState;
        private KeyboardState prevKeyState;

        // Set first state
        State gameState = State.Menu;

        // Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            // set size of screen
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 550;
            graphics.PreferredBackBufferHeight = 675;

            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;

            this.Window.Title = "Space Shooter";

            Content.RootDirectory = "Content";

            enemyBulletDamage = 10;
            menuImage = null;
            gameoverImage = null;
            color = Color.White;
        }

        // Init
        protected override void Initialize()
        {
            base.Initialize();
        }

        // Load Content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            starField.LoadContent(Content);
            hud.LoadContent(Content);
            sm.LoadContent(Content);
            //menuImage = Content.Load<Texture2D>("menuImage");
            //gameoverImage = Content.Load<Texture2D>("gameoverImage");
            mainMenuItemsFont = Content.Load<SpriteFont>("MenuItem");
            gameoverMenuItemFont = Content.Load<SpriteFont>("MenuItem");
            mainMenuTitleFont = Content.Load<SpriteFont>("MainMenuTitle");
            
        }

        // Unload Content
        protected override void UnloadContent()
        {

        }

        // Update
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit (only for gamepad)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (gameState)
            {
                case State.Playing:
                    {
    
                        checkEnemyCollisions(gameTime);

                        checkAsteroidCollisions(gameTime);

                        // Update explosions
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }

                        // If player's health hits 0, then go to gameover state
                        if (player.getHealth() <= 0)
                            gameState = State.Gameover;

                        hud.Update(gameTime);
                        player.Update(gameTime);
                        starField.Update(gameTime);

                        LoadAsteroids();
                        LoadEnemies();
                        ManageExplosions();

                        break;
                    }
                // Update Menu state
                case State.Menu:
                    {
                        // Get keyboard state
                        keyState = Keyboard.GetState();

                        // Highlight menu item
                        menuItems = new List<string>();

                        //Add each menu item to the list
                        menuItems.Add("Play");
                        menuItems.Add("Exit");

                        //Checks for input from user that represent going up
                        if (checkKeyboard(Keys.Up) || checkKeyboard(Keys.W))
                        {
                            //Will not go up further than top list item, wraps to bottom item
                            if (selected > 0)
                                selected--;
                            else if (selected == 0)
                                selected = menuItems.Count - 1;
                        }
                        //Checks for input from user that represent going down
                        if (checkKeyboard(Keys.Down) || checkKeyboard(Keys.S))
                        {
                            //Will not go lower than bottom list item, wraps to top item
                            if (selected < menuItems.Count - 1)
                                selected++;
                            else if (selected == (menuItems.Count - 1))
                                selected = 0;
                        }

                        if (checkKeyboard(Keys.Enter))
                        {
                            gameState = State.Playing;

                            //play background music
                            //MediaPlayer.Play(sm.backgroundMusic);
                        }

                        //Checks for input from user that represents 'enter'
                        if (checkKeyboard(Keys.Enter) || checkKeyboard(Keys.Space))
                        {
                            switch (selected)
                            {
                                case 0:
                                    {
                                        gameState = State.Playing;
                                        break;
                                    }
                                case 1:
                                    this.Exit();
                                    break;
                            }
                        }

                        starField.Update(gameTime);

                        break;
                    }
                // Update Gameover state
                case State.Gameover:
                    {
                        // Get keyboard state
                        keyState = Keyboard.GetState();

                        // If in gameover screen, return to main menu if SPACE or ENTER is hit
                        if (checkKeyboard(Keys.Space) || checkKeyboard(Keys.Enter))
                        {
                            // remove all objects from screen
                            enemyList.Clear();
                            asteroidList.Clear();
                            player.getBulletList().Clear();
                            explosionList.Clear();

                            // reset player health and position
                            player.setHealth(200);
                            player.setPosition(new Vector2(275 - 32, 600));

                            // reset score
                            hud.playerScore = 0;

                            gameState = State.Menu;
                        }

                        // Stop music
                        //MediaPlayer.Stop();

                        break;
                    }
            }
            prevKeyState = keyState;

            base.Update(gameTime);
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // between begin and end, draw all sprites on screen
            spriteBatch.Begin();

            switch (gameState)
            {
                    // Drawing playing state
                case State.Playing:
                    {
                        starField.Draw(spriteBatch);
                        player.Draw(spriteBatch);

                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);
                        }

                        foreach (Asteroid a in asteroidList)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Enemy e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }

                        hud.Draw(spriteBatch);
                        break;
                    }
                    // Drawing menu sate
                case State.Menu:
                    {
                        padding = 3;
                        starField.Draw(spriteBatch);
                        //spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);
                        for (int i = 0; i < menuItems.Count; i++)
                        {
                            if (i == selected)
                                color = Color.Yellow;
                            else
                                color = Color.White;
                            spriteBatch.DrawString(mainMenuItemsFont, menuItems[i], new Vector2((screenWidth / 2) - (mainMenuItemsFont.MeasureString(menuItems[i]).X / 2),
                                 (screenHeight / 2) - (mainMenuItemsFont.LineSpacing * menuItems.Count / 2) + ((mainMenuItemsFont.LineSpacing + padding) * i)), color);
                        }

                        spriteBatch.DrawString(mainMenuTitleFont, "Space Shooter", new Vector2((screenWidth / 2) - (mainMenuTitleFont.MeasureString("Space Shooter").X / 2),
                            (screenHeight / 2) - 100), Color.White);
                        break;
                    }
                    // Drawing gameover state
                case State.Gameover:
                    {
                        spriteBatch.DrawString(gameoverMenuItemFont, "Game Over", new Vector2((screenWidth / 2) - (gameoverMenuItemFont.MeasureString("Game Over").X / 2),
    (screenHeight / 2) - 50), Color.White);
                        spriteBatch.DrawString(hud.playerScoreFont, "Score: " + hud.playerScore, new Vector2((screenWidth / 2) - (hud.playerScoreFont.MeasureString("Score: " + hud.playerScore).X / 2),
    (screenHeight / 2)), Color.White);
                        break;
                    }
            }
                    spriteBatch.End();

                    base.Draw(gameTime);
        }

        //Load Asteroids
        private void LoadAsteroids()
        {
            // set random x and y positions for spawning asteroids
            int randomX = random.Next(0, 500);
            int randomY = random.Next(-500, -50);

            // if less than 5 asteroids on screen, create more until there are 5
            if (asteroidList.Count() < 5)
            {
                asteroidList.Add(new Asteroid(Content.Load<Texture2D>("asteroid"), new Vector2(randomX, randomY)));
            }

            // if any asteroids in the list were destroyed/invisible, remove them from list
            for (int i = 0; i < asteroidList.Count; i++)
            {
                if (!asteroidList[i].getVisible())
                {
                    asteroidList.RemoveAt(i);
                    i--;
                }
            }
        }

        //Load Enemy ships
        private void LoadEnemies()
        {
            // set random x and y positions for spawning enemies
            int randomX = random.Next(0, 500);
            int randomY = random.Next(-500, -50);

            // if less than 3 enemies on screen, create more until there are 3
            if (enemyList.Count() < 3)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("enemyship"), new Vector2(randomX, randomY), Content.Load<Texture2D>("enemybullet")));
            }

            // if any enemies in the list were destroyed/invisible, remove them from list
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!enemyList[i].getVisible())
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Manage explosions
        private void ManageExplosions()
        {
            // if any explosions in the list are invisible, remove them from list
            for (int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        }

        private void checkEnemyCollisions(GameTime gameTime)
        {
            // updating enemies and checking collisions of enemy to player
            foreach (Enemy e in enemyList)
            {
                List<Bullet> enemyBulletList = e.getBulletList();
                List<Bullet> playerBulletList = player.getBulletList();
                Rectangle enemyBoundingBox = e.getBoundingBox();
                Rectangle playerBoundingBox = player.getBoundingBox();

                //check if enemy is colliding with player
                //if so, set enemy invisible & subtract health from player
                if (enemyBoundingBox.Intersects(playerBoundingBox))
                {
                    player.setHealth(player.getHealth() - 10);
                    e.setVisible(false);

                    // create explosion
                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(e.getPosition().X + (e.getTexture().Width / 2), e.getPosition().Y + (e.getTexture().Height))));

                    // play explosion sound effect
                    sm.explosionSound.Play();
                }

                // Iterate through bulletlist
                // if any enemy bullets come into contact with playership, destroy bullet and subtract health from player
                for (int i = 0; i < enemyBulletList.Count; i++)
                {
                    if (playerBoundingBox.Intersects(enemyBulletList[i].getBoundingBox()))
                    {
                        enemyBulletList[i].setVisible(false);
                        player.setHealth(player.getHealth() - enemyBulletDamage);
                    }
                }

                // check player bullet collision to enemy ship
                for (int i = 0; i < playerBulletList.Count; i++)
                {
                    if (playerBulletList[i].getBoundingBox().Intersects(e.getBoundingBox()))
                    {
                        playerBulletList[i].setVisible(false);
                        e.setVisible(false);

                        // increment player score
                        hud.playerScore += 100;

                        // create explosion
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(e.getPosition().X + (e.getTexture().Width / 2), e.getPosition().Y + (e.getTexture().Height))));

                        // play explosion sound effect
                        sm.explosionSound.Play();
                    }
                }

                e.Update(gameTime);
            }
        }

        private void checkAsteroidCollisions(GameTime gameTime)
        {
            // for each asteroid in list, update it and check for collisions
            foreach (Asteroid a in asteroidList)
            {
                List<Bullet> playerBulletList = player.getBulletList();
                Rectangle asteroidBoundingBox = a.getBoundingBox();

                // check to see if asteroid collides with player
                // if so, set isVisible to false and will be removed from asteroid list
                // additionally, subtract health from player
                if (asteroidBoundingBox.Intersects(player.getBoundingBox()))
                {
                    // create explosion
                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(a.getPosition().X + (a.getTexture().Width / 2), a.getPosition().Y + (a.getTexture().Height))));

                    // play explosion sound effect
                    sm.explosionSound.Play();

                    player.setHealth(player.getHealth() - 5);
                    a.setVisible(false);
                }

                // Iterate through bulletlist. if any asteroids come into contact with bullet, destroy bullet and asteroid
                for (int i = 0; i < playerBulletList.Count; i++)
                {
                    if (asteroidBoundingBox.Intersects(playerBulletList[i].getBoundingBox()))
                    {
                        a.setVisible(false);
                        playerBulletList.ElementAt(i).setVisible(false);

                        // increment playerscore
                        hud.playerScore += 50;

                        // create explosion
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(a.getPosition().X + (a.getTexture().Width / 2), a.getPosition().Y + (a.getTexture().Height))));

                        // play explosion sound effect
                        sm.explosionSound.Play();
                    }
                }

                a.Update(gameTime);
            }
        }

        public bool checkKeyboard(Keys key)
        {
            return (keyState.IsKeyDown(key) && !prevKeyState.IsKeyDown(key));
        }
    }
}
