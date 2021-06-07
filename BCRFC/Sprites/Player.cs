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
    public class Player : Sprite
    {
        public bool HasBied = false;
        public Bullet Bullet;
        public Weapon Weapon;
        const float _delay = 2;
        float _remainingDelay = _delay;
        public bool CanAttack = true;
        private List<Pickup> Pickups = new List<Pickup>(); // maybe pickups add to this list
        private Pickup picked;
        public List<Pickup> TempInventory = new List<Pickup>(); // temporary make class and either use in player or have Pickup deal with inserting Item not Pickup see it does cause issue
        private bool CanAct = true;// to clean and give one command at time
        public List<Inventory> Inventories; // make private for eaiser use but use methods and give items to player also check naming
        public Item[] equipped = new Item[10]; // helmet chest legs primary secondary back ring ... (ring amulet special?)10

        public Player(Texture2D texture) : base(texture)
        {
            Position = new Vector2(30, 40);
            Inventories = new List<Inventory>();
            Inventory pockets = new Inventory("Pockets", 4, 1);
            Inventories.Add(pockets);
            StarterKit();
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

                if (sprite.GetType() == typeof(Pickup) && InRange(sprite)) // check for range so maybe in pickup the player has enough to deal with
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
                    TempInventory.Add(picked);
                    Pickups.Remove(picked); // maybe redundant because list is always cleared
                    picked.IsRemoved = true; // remove from map
                    Debug.WriteLine(picked.Name);
                    Debug.WriteLine(TempInventory.Count);
                    CanAct = false;
                }
            }
            // move to other place for input
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

        public bool InRange(Sprite sprite)
        {
            int Range = 72;
            return sprite.Position.X < this.Position.X + Range && sprite.Position.X > this.Position.X - Range && sprite.Position.Y < this.Position.Y + Range && sprite.Position.Y > this.Position.Y - Range; // check for better with Pos
        }

        public void StarterKit() // maybe keep here
        {
            Inventory inv = Inventories.ElementAt(0);
            Item item = new Item() { Name = "Hatchet", Description = "The Starter Hatchet" }; // modify
            inv.TryAddItem(item, 0, 0);
            item = new Item() { Name = "Sword", Description = "The Starter Sword" };
            inv.TryAddItem(item, 1, 0);
        }

        // why not
        public Item SwapEquipped(Item item, int i)
        {
            Item bufferItem = equipped[i];
            equipped[i] = item;
            return bufferItem;
        }
    }
}
