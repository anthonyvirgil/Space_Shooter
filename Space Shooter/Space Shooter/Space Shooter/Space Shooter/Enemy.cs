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
    public class Enemy
    {
        private Rectangle boundingBox;
        private Texture2D texture;
        private Texture2D bulletTexture;
        private Vector2 position;
        private int health, speed, bulletDelay, currentDifficultyLevel;
        private bool isVisible;
        private List<Bullet> bulletList;

        // Constructor
        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture)
        {
            bulletList = new List<Bullet>();
            texture = newTexture;
            bulletTexture = newBulletTexture;
            health = 2;
            position = newPosition;
            currentDifficultyLevel = 1;
            bulletDelay = 100;
            speed = 5;
            isVisible = true;
        }
        
        public Rectangle getBoundingBox()
        {
            return boundingBox;
        }

        public void setBoundingBox(Rectangle boundingBox)
        {
            this.boundingBox = boundingBox;
        }

        public int getHealth()
        {
            return health;
        }

        public void setHealth(int health)
        {
            this.health = health;
        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public bool getVisible()
        {
            return isVisible;
        }

        public void setVisible(bool isVisible)
        {
            this.isVisible = isVisible;
        }

        public List<Bullet> getBulletList()
        {
            return bulletList;
        }

        public void setBulletList(List<Bullet> bulletList)
        {
            this.bulletList = bulletList;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Update collision rectangle
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            // Update enemy movement
            position.Y += speed; // moving up and down

            // Move enemy back to top to screen if reaches past bottom
            if (position.Y >= 675)
                position.Y = -75;

            EnemyShoot();
            UpdateBullets();
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw enemy ship on screen
            spriteBatch.Draw(texture, position, Color.White);

            // draw enemy bullets on screen
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        // Update bullet function
        public void UpdateBullets()
        {
            // for each bullet in list, update movement
            foreach (Bullet b in bulletList)
            {
                // set movement for bullet
                b.setYPosition(b.getPosition().Y + b.getSpeed());

                Vector2 bulletPosition = b.getPosition();
                Texture2D bulletTexture = b.getTexture();

                // set boundingbox for each bullet in bullet list
                b.setBoundingBox(new Rectangle((int)bulletPosition.X, (int)bulletPosition.Y, bulletTexture.Width, bulletTexture.Height));

                // if bullet hits top of screen, set visible to false
                if (b.getPosition().Y >= 675)
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

        // Enemy shoot function
        public void EnemyShoot()
        {
            // Shoot only if bullet delay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            if (bulletDelay <= 0)
            {
                // create new bullet and position it in front center of enemy ship
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.setPosition(new Vector2(position.X + texture.Width / 2 - newBullet.getTexture().Width / 2, position.Y + 30));

                newBullet.setVisible(true);

                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);
            }

            // reset bullet delay
            if (bulletDelay == 0)
                bulletDelay = 40;
        }
    }
}
