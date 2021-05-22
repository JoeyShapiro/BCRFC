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
    class Slime : Enemy
    {
        private List<Sprite> enemies;
        private bool isFresh = true;

        public Slime(Texture2D texture) : base(texture)
        {
            Position = new Vector2(500, 40);
            enemies = new List<Sprite>();
            Health = 4;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            if (isFresh)
                foreach (var sprite in sprites.ToArray())
                    if (sprite.GetType() == typeof(Player))
                    {
                        enemies.Add(sprite);
                        isFresh = false;
                    }
            Move(sprites);

            Move(sprites);

            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;

                if ((this.Velocity.X > 0 && this.IsTouchingLeft(sprite)) ||
                    (this.Velocity.X < 0 & this.IsTouchingRight(sprite)))
                    this.Velocity.X = 0;

                if ((this.Velocity.Y > 0 && this.IsTouchingTop(sprite)) ||
                    (this.Velocity.Y < 0 & this.IsTouchingBottom(sprite)))
                    this.Velocity.Y = 0;
            }

            // move to Sprite
            if (Health <= 0)
                IsRemoved = true;

            Position += Velocity;

            Velocity = Vector2.Zero;
        }

        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public override void Move(List<Sprite> sprites)
        {
            foreach (var sprite in enemies.ToArray())
            {
                if (sprite.Position.X > Position.X)
                    Velocity.X = Speed;
                else if (sprite.Position.X < Position.X)
                    Velocity.X = -Speed;
                if (sprite.Position.Y > Position.Y)
                    Velocity.Y = Speed;
                else if (sprite.Position.Y < Position.Y)
                    Velocity.Y = -Speed;
            }
        }
    }
}
