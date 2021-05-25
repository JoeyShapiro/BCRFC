using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BCRFC.Sprites;
using BCRFC.Models;

namespace BCRFC.Sprites
{
    public class Bullet : Sprite
    {
        private float timer;
        public Bullet(Texture2D texture) : base(texture)
        {

        }

        public void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }
    }
}
