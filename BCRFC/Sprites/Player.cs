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
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position.Y += 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Position.X -= 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position.X += 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                AddBullet(sprites);
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
    }
}
