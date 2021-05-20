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
            Position = new Vector2(30, 40);
            enemies = new List<Sprite>();
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
                    Position.X++;
                else if (sprite.Position.X < Position.X)
                    Position.X--;
                if (sprite.Position.Y > Position.Y)
                    Position.Y++;
                else if (sprite.Position.Y < Position.Y)
                    Position.Y--;
            }
        }
    }
}
