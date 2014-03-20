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
    public class Explosion
    {
        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }
        public float timer { get; set; }
        public float interval { get; set; }
        public Vector2 origin { get; set; }
        public int currentFrame { get; set; }
        public int spriteWidth { get; set; }
        public int spriteHeight { get; set; }
        public Rectangle sourceRectangle { get; set; }
        public bool isVisible { get; set; }

        // Constructor
        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 20f;
            currentFrame = 1;
            spriteWidth = 128;
            spriteHeight = 128;
            isVisible = true;
        }

        // Load Content
        public void LoadContent(ContentManager content)
        {
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // increase timer by number of milliseconds since update was last called
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // check that time is more than chosen interval
            if (timer > interval)
            {
                //show next frame
                currentFrame++;

                //reset timer to 0
                timer = 0f;
            }

            // if on last frame of animation, make explosion invisible and set current frame to beginning of sprite sheet
            if (currentFrame == 17)
            {
                currentFrame = 0;
                isVisible = false;
            }

            //grabbing each sprite from the image
            sourceRectangle = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);

        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // if explosion is visible, then draw
            if (isVisible)
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
