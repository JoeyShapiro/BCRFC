using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BCRFC.Sprites;
using BCRFC.Models;
using System.Linq;

namespace BCRFC
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<Sprite> _sprites;
        Texture2D _texture;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        protected float timer;
     
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.Window.AllowUserResizing = false;
            this.Window.Title = "TTT";

            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _texture = Content.Load<Texture2D>("ball");

            _sprites = new List<Sprite>()
            {
                new Player(_texture)
                {
                    Bullet = new Bullet(Content.Load<Texture2D>("Bullet"))
                }
            };
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var sprite in _sprites)
                sprite.Update(gameTime, _sprites);

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var sprite in _sprites.ToArray())
                sprite.Update(gameTime, _sprites);

            PostUpdate(gameTime);

            base.Update(gameTime);
        }

        private void PostUpdate(GameTime gameTime)
        {
            //var collidableSprites = _sprites.Where(c => c is ICollidable);

            //foreach (var spriteA in collidableSprites)
            //{
            //    foreach (var spriteB in collidableSprites)
            //    {
            //        if (spriteA == spriteB)
            //            continue;

            //        if (!spriteA.CollisionArea.Intersects(spriteB.CollisionArea))
            //            continue;

            //        if (spriteA.Intersects(spriteB))
            //            ((ICollidable)spriteA).OnCollide(spriteB);
            //    }
            //}

            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
