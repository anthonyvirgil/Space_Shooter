using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Space_Shooter
{
    public class SoundManager
    {
        // mp3 is Song object
        // wav is SoundEffect
        public SoundEffect playerShootSound { get; set; }
        public SoundEffect explosionSound { get; set; }
        public Song backgroundMusic { get; set; }
        
        // constructor
        public SoundManager()
        {
            playerShootSound = null;
            explosionSound = null;
            backgroundMusic = null;
        }

        // load content
        public void LoadContent(ContentManager content)
        {
            playerShootSound = content.Load<SoundEffect>("playershoot");
            explosionSound = content.Load<SoundEffect>("explode");
            backgroundMusic = content.Load<Song>("theme");
        }
    }
}
