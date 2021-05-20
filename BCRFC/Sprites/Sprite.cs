using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BCRFC.Models;
using BCRFC.Sprites;

namespace BCRFC.Sprites
{
    public class Sprite : ICollidable
    {
        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        public Vector2 Position;
        public Vector2 Direction;
        public Vector2 Origin;
        public Color Color = Color.White;
        public float Velocity = 0f;
        public Sprite Parent;
        public float LifeSpan = 0f;
        protected float _rotation;
        public float LinearVelocity = 4f;

        protected Texture2D _texture;

        public bool IsRemoved = false;

        public Sprite(Texture2D texture)
        {
            _texture = texture;

            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public Rectangle Rectangle
        {
            get
            {
                if(_texture != null)
                    return new Rectangle((int) Position.X - (int)Origin.X, (int) Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
                throw new Exception("Unknown sprite");
            }
        }

        //public Rectangle CollisionArea
        //{
        //    get
        //    {
        //        return new Rectangle(Rectangle.X, Rectangle.Y, MathHelper.Max(Rectangle.Width, Rectangle.Height), MathHelper.Max(Rectangle.Width, Rectangle.Height));
        //    }
        //}

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
            }
        }

        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                    Matrix.CreateRotationZ(_rotation) *
                    Matrix.CreateTranslation(new Vector3(Position, 0));
            }
        }

        public virtual void Update (GameTime GameTime, List<Sprite> sprite)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, Position, null, Color.White, _rotation, Origin, 1, SpriteEffects.None, 0);
        }

        //public bool Intersects(Sprite sprite)
        //{
        //    if (this.TextureData == null)
        //        return false;
        //    if (sprite.TextureData == null)
        //        return false;

        //    var transformAtoB = this.Transform * Matrix.Invert(sprite.Transform);

        //    var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAtoB);
        //    var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAtoB);

        //    var yPosInB = Vector2.Transform(Vector2.Zero, transformAtoB);

        //    for (int yA = 0; yA < this.Rectangle.Height; yA++)
        //    {
        //        var posInB = yPosInB;

        //        for (int xA = 0; xA < this.Rectangle.Width; xA++)
        //        {
        //            var xB = (int)Math.Round(posInB.X);
        //            var yB = (int)Math.Round(posInB.Y);
        //            if (0 <= xB && xB < sprite.Rectangle.Width &&
        //                0 <= yB && yB < sprite.Rectangle.Height)
        //            {
        //                var colorA = this.TextureData[xA + yA * this.Rectangle.Width];
        //                var colorB = sprite.TextureData[xB + yB * sprite.Rectangle.Width];

        //                if(colorA.A != 0 && colorB.A != 0)
        //                {
        //                    return true;
        //                }
        //            }

        //            posInB += stepX;
        //        }
        //        yPosInB += stepY;
        //    }

        //    return false;
        //}

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void OnCollide(Sprite sprite)
        {
            throw new NotImplementedException();
        }
    }
}