using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BCRFC.Sprites;
using BCRFC.Models;
using System.Diagnostics;
using System.Linq;

namespace BCRFC.Sprites
{
    class Player : Sprite
    {
        public bool HasBied = false;
        public Bullet Bullet;
        public Weapon Weapon;
        const float _delay = 2;
        float _remainingDelay = _delay;
        public bool CanAttack = true;
        private List<Pickup> Pickups = new List<Pickup>(); // maybe pickups add to this list
        private Pickup picked;
        public List<Pickup> Inventory = new List<Pickup>(); // temporary make class and either use in player or have Pickup deal with inserting Item not Pickup
        private bool CanAct = true;// to clean and give one command at time

        public Player(Texture2D texture) : base(texture)
        {
            Position = new Vector2(30, 40);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move(sprites);
            Pickups.Clear(); // clear the list why does game code seem so excessive

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

                if (sprite.GetType() == typeof(Pickup)) // check for range so maybe in pickup the player has enough to deal with
                    Pickups.Add((Pickup) sprite);
            }

            if (IsOutOfBoundsX())
                this.Velocity.X = 0;
            if (IsOutOfBoundsY())
                this.Velocity.Y = 0;

            float timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                LifeSpan--;
                if (LifeSpan <= 0)
                {
                    CanAttack = true;
                    CanAct = true; // maybe best but make method
                }

                _remainingDelay = _delay;
            }

            Position += Velocity;

            Velocity = Vector2.Zero;
        }

        public void Move(List<Sprite> sprites)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Velocity.Y = -Speed;
                Rotation = MathHelper.ToRadians(90);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Velocity.Y = Speed;
                Rotation = MathHelper.ToRadians(270);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Velocity.X = -Speed;
                Rotation = MathHelper.ToRadians(180);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Velocity.X = Speed;
                Rotation = MathHelper.ToRadians(0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (CanAttack)
                    Attack(sprites);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                picked = Pickups.FirstOrDefault();
                if (picked != null && CanAct)
                {
                    Inventory.Add(picked);
                    Pickups.Remove(picked); // maybe redundant because list is always cleared
                    picked.IsRemoved = true; // remove from map
                    Debug.WriteLine(picked.Name);
                    Debug.WriteLine(Inventory.Count);
                    CanAct = false;
                }
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
            Debug.WriteLine("Attack");
            var weapon = Weapon.Clone() as Weapon;
            weapon.Attacked = new List<Sprite>();
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
            CanAttack = false;

            sprites.Add(weapon);
        }
    }
}
