using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BCRFC.Sprites;
using BCRFC.Models;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace BCRFC.States
{
    public abstract class State
    {
        protected Game1 _game;
        protected ContentManager _content;

        public State(Game1 game, ContentManager content)
        {
            _game = game;
            _content = content;
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void PostUpdate(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
