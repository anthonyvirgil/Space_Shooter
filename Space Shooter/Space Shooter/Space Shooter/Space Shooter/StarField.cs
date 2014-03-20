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
    public class StarField
    {
        private Texture2D texture;
        private Vector2 bgPosition1, bgPosition2;
        public int speed { get; set; }

        // Constructor
        public StarField()
        {
            texture = null;
            bgPosition1 = new Vector2(0, 0);
            bgPosition2 = new Vector2(0, -675);
            speed = 5;
        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("space");
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bgPosition1, Color.White);
            spriteBatch.Draw(texture, bgPosition2, Color.White);
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Set speed for background scrolling
            bgPosition1.Y = bgPosition1.Y + speed;
            bgPosition2.Y = bgPosition2.Y + speed;

            // Scrolling background (repeating)
            if (bgPosition1.Y >= 675)
            {
                bgPosition1.Y = 0;
                bgPosition2.Y = -675;
            }
        }
    }
}
