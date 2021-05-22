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
        private int Damage;
        public List<Sprite> Attacked; // is this best way to attack EACH enemy ONLY once

        public Weapon(Texture2D texture) : base(texture)
        {
            Damage = 2;
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
                this.Position.Y = Parent.Position.Y - 64;
            }
            else if (Parent.Rotation == MathHelper.ToRadians(270))
            {
                this.Position.X = Parent.Position.X;
                this.Position.Y = Parent.Position.Y + 64;
            }
            else if (Parent.Rotation == MathHelper.ToRadians(180))
            {
                this.Position.X = Parent.Position.X - 64;
                this.Position.Y = Parent.Position.Y;
            }
            else if (Parent.Rotation == MathHelper.ToRadians(0))
            {
                this.Position.X = Parent.Position.X + 64;
                this.Position.Y = Parent.Position.Y;
            }

            foreach (var sprite in sprites)
            {
                if (sprite == this || sprite == Parent)
                    continue;

                if (this.IsTouchingLeft(sprite) ||
                    this.IsTouchingRight(sprite) ||
                    this.IsTouchingTop(sprite) ||
                    this.IsTouchingBottom(sprite))
                    this.Attack(sprite);
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

        public void Attack(Sprite sprite)
        {
            // spend time cleaning this up
            if (Attacked.Contains(sprite) || sprite.IsInvulnerable)
                return;
            Debug.WriteLine("Attacked");
            sprite.Health -= Damage;
            sprite.IsInvulnerable = true;
            Attacked.Add(sprite);
        }
    }
}
