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
        public List<Sprite> sprites;
        private Texture2D power;
        public static Random random;
        private Texture2D gameBackgroundTexture;
        private DelayedAction ToggleBools;
        const float _delay = 0.2f;
        float _remainingDelay = _delay;
        private KeyboardState oldState; // find cleaner way
        public Player player;

        public GameState(Game1 game, ContentManager content) : base(game, content)
        {
            random = new Random();

            //ToggleBools = new DelayedAction(delegate {
            //}, 0.2f); // i think all same delay but try for other delays later
        }

        public override void LoadContent()
        {
            var playerTexture = _content.Load<Texture2D>("Player");
            //font = _content.Load<SpriteFont>("Font");
            player = new Player(playerTexture)
            {
                Bullet = new Bullet(_content.Load<Texture2D>("Bullet")),
                Weapon = new Weapon(_content.Load<Texture2D>("Weapon")),
                Speed = 5
            };
            sprites = new List<Sprite>
            {
                player,
                new Slime(playerTexture)
                {
                    Speed = 3
                },
                new Pickup(playerTexture)
                {
                    Name = "Not PH"
                }
            };
        }

        public override void Update(GameTime gameTime)
        {
            float timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState newState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            if (oldState.IsKeyUp(Keys.I) && newState.IsKeyDown(Keys.I))
                _game.TogglePlayerForm(player);

            foreach (var sprite in sprites.ToArray())
                sprite.Update(gameTime, sprites);


            foreach (var sprite in sprites.ToArray())
                sprite.Update(gameTime, sprites);

            oldState = newState;
            //ToggleBools.Update(timer); look into makes it blinky
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