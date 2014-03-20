using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Space_Shooter
{
    // Main
    public class Player
    {
        private Texture2D texture;
        private Vector2 position;
        private int speed;

        // Collision variables
        private Rectangle boundingBox;
        private bool isColliding;

        // Bullet variables
        private Texture2D bulletTexture;
        private float bulletDelay;
        private List<Bullet> bulletList;

        // Health
        private int health;

        SoundManager sm = new SoundManager();

        public static Player player;

        // Constructor
        public Player()
        {
            texture = null;
            position = new Vector2(275 - 32, 600);
            speed = 6;
            isColliding = false;
            bulletTexture = null;
            bulletList = new List<Bullet>();
            bulletDelay = 1;
            health = 200;
            player = this;
        }

        public int getHealth()
        {
            return health;
        }

        public void setHealth(int health)
        {
            this.health = health;
        }

        public Rectangle getBoundingBox()
        {
            return boundingBox;
        }

        public void setBoundingBox(Rectangle boundingBox)
        {
            this.boundingBox = boundingBox;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public List<Bullet> getBulletList()
        {
            return bulletList;
        }

        // Load Content
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("playership");
            bulletTexture = content.Load<Texture2D>("playerbullet");
            sm.LoadContent(content);
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw player sprite
            spriteBatch.Draw(texture, position, Color.White);

            // draw each bullet in bulletList
            foreach (Bullet b in bulletList)
                b.Draw(spriteBatch);
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // checks for keyboard input every frame
            KeyboardState keyState = Keyboard.GetState();

            // Ship controls
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                position.Y = position.Y - speed;
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                position.Y = position.Y + speed;
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                position.X = position.X - speed;
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                position.X = position.X + speed;

            // Keep player ship in screen bounds
            if (position.X <= 0)
                position.X = 0;
            if (position.X >= 550 - texture.Width)
                position.X = 550 - texture.Width;
            if (position.Y <= 0)
                position.Y = 0;
            if (position.Y >= 675 - texture.Height)
                position.Y = 675 - texture.Height;

            // create boundingbox for playership
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            // Fire bullets
            if (keyState.IsKeyDown(Keys.Space))
            {
                Shoot();               
            }

            // allows for shooting bullets as you tap spacebar
            if (keyState.IsKeyUp(Keys.Space))
               bulletDelay = 1;

            // update bullets already on screen
            UpdateBullets();
        }

        // Shoot method (used to set starting position of bullets)
        public void Shoot()
        {
            // Shoot only if bullet delay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            // If bullet delay is at 0, create new bullet at player position and make visible on screen, then add bullet to list
            if (bulletDelay <= 0)
            {
                // play sound effect
                sm.playerShootSound.Play();

                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.setPosition(new Vector2(position.X + (texture.Width/2) - (newBullet.getTexture().Width / 2), position.Y));

                newBullet.setVisible(true);

                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);
            }

            // reset bullet delay
            if (bulletDelay == 0)
                bulletDelay = 20;
        }

        // Update bullet function
        public void UpdateBullets()
        {
            // for each bullet in list, update movement
            foreach (Bullet b in bulletList)
            {
                // set movement for bullet
                b.setYPosition(b.getPosition().Y - b.getSpeed());

                Vector2 bulletPosition = b.getPosition();
                Texture2D bulletTexture = b.getTexture();

                // set boundingbox for each bullet in bullet list
                b.setBoundingBox(new Rectangle((int)bulletPosition.X, (int)bulletPosition.Y, bulletTexture.Width, bulletTexture.Height));

                // if bullet hits top of screen, set visible to false
                if (b.getPosition().Y <= 0)
                    b.setVisible(false);
            }

            // iterate through bullet list. if bullet is not visible, remove from list
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].getVisible())
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}

