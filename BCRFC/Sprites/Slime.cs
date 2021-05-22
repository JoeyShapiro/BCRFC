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
    class Slime : Enemy
    {
        private List<Sprite> enemies;
        private bool isFresh = true;
        const float _delay = 2;
        float _remainingDelay = _delay;

        public Slime(Texture2D texture) : base(texture)
        {
            Position = new Vector2(500, 40);
            enemies = new List<Sprite>();
            Health = 4;
            MaxHealth = Health; // maybe set first idk im tired
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

                Repulse(sprite);
            }

            // move to Sprite
            if (Health <= 0)
            {
                if (!IsRemoved) // i hate checking everything i need function
                    Spawn(sprites);
                IsRemoved = true;
            }

            // give invuln to deal with not spawning at start
            float timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                IsInvulnerable = false; // find better place im tired of these checks

                _remainingDelay = _delay;
            }

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

        public void Spawn(List<Sprite> sprites)
        {
            if (MaxHealth < 4)
                return;

            var slime = this.Clone() as Slime;
            slime.Position = this.Position;
            slime.Health = MaxHealth / 2;
            slime.MaxHealth = slime.Health;
            slime.IsRemoved = false;
            slime.isFresh = true;

            Debug.WriteLine("new Slimes 2 " + slime.MaxHealth + " at " + slime.Position.X + ", " + slime.Position.Y); // they get attacked when spawned and are on each other
            sprites.Add(slime);
            sprites.Add(slime);
        }
    }
}
