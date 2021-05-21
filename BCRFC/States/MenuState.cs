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

namespace BCRFC.States
{
    class MenuState : State
    {
        private List<Component> _components;
        //private Texture2D menuBackgroundTexture;

        public MenuState(Game1 game, ContentManager content) : base(game, content)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(menuBackgroundTexture, new Vector2(0, 0), Color.White);
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void LoadContent()
        {
            //var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            //var buttonFont = _content.Load<SpriteFont>("ButtonFonts/Font");
            //menuBackgroundTexture = _content.Load<Texture2D>("Backgrounds/Menu");

            _components = new List<Component>() 
            { 
                //new Button(buttonTexture, buttonFont)
                //{
                //    Text = "AAA",
                //    Position = new Vector2(Game1.ScreenWidth / 2, 400),
                //    Click = new EventHandler(Button_Start_Clicked),
                //    // layer = 0.1f
                //},
            };
        }

        private void Start()
        {
            _game.ChangeState(new GameState(_game, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
