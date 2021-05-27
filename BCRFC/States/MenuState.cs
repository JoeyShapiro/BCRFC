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
using GeonBit.UI.Entities;
using GeonBit.UI;

namespace BCRFC.States
{
    class MenuState : State
    {
        private List<Component> _components;
        //private Texture2D menuBackgroundTexture;
        private GameState gameState;

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
            MainMenu();
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

        public void MainMenu()
        {
            // start button
            Button button = new Button(text: "Start", anchor: Anchor.TopCenter, size: new Vector2(256, 32), offset: new Vector2(0, 256));
            UserInterface.Active.AddEntity(button);
            // codex button
            Button btnCodex = new Button(text: "Codex", anchor: Anchor.TopCenter, size: new Vector2(256, 32), offset: new Vector2(0, 256+32));
            UserInterface.Active.AddEntity(btnCodex);
            // options button
            Button btnOptions = new Button(text: "Options", anchor: Anchor.TopCenter, size: new Vector2(256, 32), offset: new Vector2(0, 256+64));
            UserInterface.Active.AddEntity(btnOptions);
            // exit button
            Button btnExit = new Button(text: "Exit", anchor: Anchor.TopCenter, size: new Vector2(256, 32), offset: new Vector2(0, 256+96));
            UserInterface.Active.AddEntity(btnExit);
            button.OnClick = (Entity btn) =>
            {
                gameState = new GameState(_game, _game.Content);
                UserInterface.Active.Clear();
                _game.ChangeState(gameState);
            };
            btnCodex.OnClick = (Entity btn) =>
            {
                UserInterface.Active.Clear();
                MenuCodex();
            };
            btnOptions.OnClick = (Entity btn) =>
            {
                UserInterface.Active.Clear();
                MenuOptions();
            };
            btnExit.OnClick = (Entity btn) =>
            {
                _game.Exit();
            };
        }

        public void MenuCodex()
        {
            Button btnBack = new Button(text: "<", size: new Vector2(96, 32), offset: new Vector2(0, 0));
            UserInterface.Active.AddEntity(btnBack);
            btnBack.OnClick = (Entity btn) =>
            {
                UserInterface.Active.Clear();
                MainMenu();
            };
        }

        public void MenuOptions()
        {
            Button btnBack = new Button(text: "<", size: new Vector2(96, 32), offset: new Vector2(0, 0));
            UserInterface.Active.AddEntity(btnBack);
            btnBack.OnClick = (Entity btn) =>
            {
                UserInterface.Active.Clear();
                MainMenu();
            };
        }
    }
}
