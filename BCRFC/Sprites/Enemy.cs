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
    public abstract class Enemy : Sprite
    {
        // make Item and Inventory in models or something also make character class for NPC and Player
        public List<int> Inventory;

        public Enemy(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        public abstract void Move(List<Sprite> sprites);
        public abstract void Attack();
    }
}
