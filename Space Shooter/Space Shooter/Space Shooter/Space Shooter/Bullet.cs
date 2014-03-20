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
    public class Bullet
    {
        private Rectangle boundingBox;
        private Texture2D texture;
        private Vector2 origin;
        private Vector2 position;
        private bool isVisible;
        private float speed;

        // Constructor
        public Bullet(Texture2D newTexture)
        {
            speed = 10;
            texture = newTexture;
            isVisible = false;
        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void setYPosition(float y)
        {
            this.position.Y = y;
        }

        public void setXPosition(float x)
        {
            this.position.X = x;
        }

        public bool getVisible()
        {
            return isVisible;
        }

        public void setVisible(bool isVisible)
        {
            this.isVisible = isVisible;
        }

        public float getSpeed()
        {
            return speed;
        }

        public void setSpeed(float speed)
        {
            this.speed = speed;
        }

        public Rectangle getBoundingBox()
        {
            return boundingBox;
        }

        public void setBoundingBox(Rectangle boundingBox)
        {
            this.boundingBox = boundingBox;
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }
}
