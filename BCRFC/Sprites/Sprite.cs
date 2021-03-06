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
        public Vector2 Velocity;
        public Sprite Parent;
        public float LifeSpan = 0f;
        protected float _rotation;
        public float LinearVelocity = 4f;
        public float Speed;
        public int Health;
        public int MaxHealth;
        public bool IsInvulnerable = false;

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
                    return new Rectangle((int) Position.X, (int) Position.Y, _texture.Width, _texture.Height);
                throw new Exception("Unknown sprite");
            }
        }

        public Rectangle CollisionArea
        {
            get
            {
                return new Rectangle(Rectangle.X, Rectangle.Y, MathHelper.Max(Rectangle.Width, Rectangle.Height), MathHelper.Max(Rectangle.Width, Rectangle.Height));
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
            }
        }

        public readonly Color[] TextureData;

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

        #region Colloision
        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Left &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
              this.Rectangle.Right > sprite.Rectangle.Right &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Top &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
              this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsOutOfBoundsX()
        {
            return this.Rectangle.Right + this.Velocity.X > 640 || this.Rectangle.Left + this.Velocity.X < 0;
        }
        protected bool IsOutOfBoundsY()
        {
            return this.Rectangle.Bottom + this.Velocity.Y > 520 || this.Rectangle.Top + this.Velocity.Y < 0;
        }

        // check if best also find more math way to solve
        protected void Repulse(Sprite sprite)
        {
            if (this.Position == sprite.Position) // i think this solves if on top of each other
                this.Position += new Vector2(new Random().Next(0, 1), new Random().Next(0, 1));
            if (IsTouchingTop(sprite))
                this.Position += new Vector2(0, -1);
            if (IsTouchingBottom(sprite))
                this.Position += new Vector2(0, 1);
            if (IsTouchingLeft(sprite))
                this.Position += new Vector2(-1, 0);
            if (IsTouchingRight(sprite))
                this.Position += new Vector2(1, 0);
        }

        #endregion

        //public bool Intersects(Sprite sprite)
        //{
        //    if (this.TextureData == null)
        //        return false;

        //    if (sprite.TextureData == null)
        //        return false;

        //    // Calculate a matrix which transforms from A's local space into
        //    // world space and then into B's local space
        //    var transformAToB = this.Transform * Matrix.Invert(sprite.Transform);

        //    // When a point moves in A's local space, it moves in B's local space with a
        //    // fixed direction and distance proportional to the movement in A.
        //    // This algorithm steps through A one pixel at a time along A's X and Y axes
        //    // Calculate the analogous steps in B:
        //    var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
        //    var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

        //    // Calculate the top left corner of A in B's local space
        //    // This variable will be reused to keep track of the start of each row
        //    var yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

        //    for (int yA = 0; yA < this.Rectangle.Height; yA++)
        //    {
        //        // Start at the beginning of the row
        //        var posInB = yPosInB;

        //        for (int xA = 0; xA < this.Rectangle.Width; xA++)
        //        {
        //            // Round to the nearest pixel
        //            var xB = (int)Math.Round(posInB.X);
        //            var yB = (int)Math.Round(posInB.Y);

        //            if (0 <= xB && xB < sprite.Rectangle.Width &&
        //                0 <= yB && yB < sprite.Rectangle.Height)
        //            {
        //                // Get the colors of the overlapping pixels
        //                var colourA = this.TextureData[xA + yA * this.Rectangle.Width];
        //                var colourB = sprite.TextureData[xB + yB * sprite.Rectangle.Width];

        //                // If both pixel are not completely transparent
        //                if (colourA.A != 0 && colourB.A != 0)
        //                {
        //                    return true;
        //                }
        //            }

        //            // Move to the next pixel in the row
        //            posInB += stepX;
        //        }

        //        // Move to the next row
        //        yPosInB += stepY;
        //    }

        //    // No intersection found
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