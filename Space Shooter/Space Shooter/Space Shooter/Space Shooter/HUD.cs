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
    public class HUD
    {
        public int playerScore { get; set; }
        public int screenWidth { get; set; }
        public int screenHeight { get; set; }
        public SpriteFont playerScoreFont { get; set; }
        public Vector2 playerScorePosition { get; set; }
        public Vector2 healthBarPosition { get; set; }
        public Texture2D healthTexture { get; set; }
        public bool showHud { get; set; }
        public Rectangle healthRectangle { get; set; }

        // constructor
        public HUD()
        {
            playerScore = 0;
            showHud = true;
            screenWidth = 550;
            screenHeight = 675;
            playerScoreFont = null;
            healthTexture = null;
            healthBarPosition = new Vector2(50, 60);
        }

        // Load content
        public void LoadContent(ContentManager content)
        {
            playerScoreFont = content.Load<SpriteFont>("Score");
            healthTexture = content.Load<Texture2D>("healthbar");
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // get keyboard state
            KeyboardState keyState = Keyboard.GetState();

            // create rectangle for health bar
            healthRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, Player.player.getHealth(), healthTexture.Height);
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            playerScorePosition = new Vector2((screenWidth / 2) - (playerScoreFont.MeasureString("Score: " + playerScore.ToString()).X / 2),30);

            // If showHud is true, then display HUD
            if (showHud)
            {
                spriteBatch.DrawString(playerScoreFont, "Score: " + playerScore, playerScorePosition, Color.White);

                // draw health bar texture
                spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
            }
        }
    }
}
