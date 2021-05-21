using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BCRFC.Sprites;
using BCRFC.Models;
using BCRFC.States;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using BCRFC.Controls;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BCRFC.States
{
    public class GameState : State
    {
        private SpriteFont font;
        private List<Sprite> sprites;
        private float timer;
        private Texture2D power;
        public static Random random;
        private Texture2D gameBackgroundTexture;

        public GameState(Game1 game, ContentManager content) : base(game, content)
        {
            random = new Random();
        }

        public override void LoadContent()
        {
            var playerTexture = _content.Load<Texture2D>("Player");
            //font = _content.Load<SpriteFont>("Font");
            sprites = new List<Sprite>
            {
                new Player(playerTexture)
                {
                    Bullet = new Bullet(_content.Load<Texture2D>("Bullet")),
                    Weapon = new Weapon(_content.Load<Texture2D>("Weapon")),
                    Speed = 5
                },
                new Slime(playerTexture)
                {
                    Speed = 3
                }
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            foreach (var sprite in sprites.ToArray())
                sprite.Update(gameTime, sprites);

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var sprite in sprites.ToArray())
                sprite.Update(gameTime, sprites);

            PostUpdate(gameTime);
        }
        public override void PostUpdate(GameTime gameTime)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].IsRemoved)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var sprite in sprites)
                sprite.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}