using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BCRFC.Sprites;
using BCRFC.Models;
using System.Diagnostics;

namespace BCRFC.Sprites
{
    class Weapon : Sprite
    {
        public Weapon(Texture2D texture) : base(texture)
        {

        }
        
        const float _delay = 2;
        float _remainingDelay = _delay;

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            // follow player
            // find cleaner way
            if (Parent.Rotation == MathHelper.ToRadians(90))
            {
                this.Position.X = Parent.Position.X;
                this.Position.Y = Parent.Position.Y - 10;
            }
            else if (Parent.Rotation == MathHelper.ToRadians(270))
            {
                this.Position.X = Parent.Position.X;
                this.Position.Y = Parent.Position.Y + 10;
            }
            else if (Parent.Rotation == MathHelper.ToRadians(180))
            {
                this.Position.X = Parent.Position.X - 10;
                this.Position.Y = Parent.Position.Y;
            }
            else if (Parent.Rotation == MathHelper.ToRadians(0))
            {
                this.Position.X = Parent.Position.X + 10;
                this.Position.Y = Parent.Position.Y;
            }

            float timer = (float) gameTime.ElapsedGameTime.TotalSeconds;

            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                LifeSpan--;
                if (LifeSpan <= 0)
                    IsRemoved = true;
                
                _remainingDelay = _delay;
            }

        }
    }
}
