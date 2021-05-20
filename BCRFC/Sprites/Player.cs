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
    class Player : Sprite
    {
        public bool HasBied = false;
        public Bullet Bullet;
        public Weapon Weapon;

        public Player(Texture2D texture) : base(texture)
        {
            Position = new Vector2(30, 40);
            Velocity = 5f;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move(sprites);
        }

        public void Move(List<Sprite> sprites)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position.Y -= 3;
                Rotation = MathHelper.ToRadians(90);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position.Y += 3;
                Rotation = MathHelper.ToRadians(270);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Position.X -= 3;
                Rotation = MathHelper.ToRadians(180);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position.X += 3;
                Rotation = MathHelper.ToRadians(0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Attack(sprites);
            }
        }

        public void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.Direction = this.Direction;
            bullet.Position = this.Position;
            bullet.LinearVelocity = this.LinearVelocity;
            bullet.LifeSpan = 2f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }

        public void Attack(List<Sprite> sprites)
        {
            var weapon = Weapon.Clone() as Weapon;
            weapon.Direction = this.Direction;
            // find cleaner way
            if (this.Rotation == MathHelper.ToRadians(90))
            {
                weapon.Position.X = this.Position.X;
                weapon.Position.Y = this.Position.Y - 10;
            } else if (this.Rotation == MathHelper.ToRadians(270))
            {
                weapon.Position.X = this.Position.X;
                weapon.Position.Y = this.Position.Y + 10;
            } else if (this.Rotation == MathHelper.ToRadians(180))
            {
                weapon.Position.X = this.Position.X - 10;
                weapon.Position.Y = this.Position.Y;
            } else if (this.Rotation == MathHelper.ToRadians(0))
            {
                weapon.Position.X = this.Position.X + 10;
                weapon.Position.Y = this.Position.Y;
            }
            weapon.Rotation = this.Rotation;
            weapon.LifeSpan = 2f;
            weapon.Parent = this;

            sprites.Add(weapon);
        }
    }
}
