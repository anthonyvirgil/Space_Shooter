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
    public class Asteroid
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 origin;
        private float rotationAngle;
        private int speed;
        private Rectangle boundingBox;
        private bool isVisible;
        Random random = new Random();

        // Constructor
        public Asteroid(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            speed = 4;
            isVisible = true;
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

        // Load Content
        public void LoadContent(ContentManager Content)
        {
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Set bounding box for collision
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            // update origin of asteroid sprite for rotation
            //origin.X = texture.Width / 2;
            //origin.Y = texture.Height / 2;

            // Update Movement
            position.Y = position.Y + speed;

            // if asteroid passed bottom of screen, spawn asteroid at random location at the top
            if (position.Y >= 675)
            {
                position.Y = -50;
                position.X = random.Next(0, 500);
            }
            // Rotating Asteroid
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //rotationAngle += elapsed;
            //float circle = MathHelper.Pi * 2;
            //rotationAngle = rotationAngle % circle;
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
                spriteBatch.Draw(texture, position, Color.White);
                //spriteBatch.Draw(texture, position, null, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
        }

    }
}
