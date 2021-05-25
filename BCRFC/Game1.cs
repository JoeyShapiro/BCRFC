using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BCRFC.Sprites;
using BCRFC.Models;
using System.Linq;
using BCRFC.States;
using BCRFC.Controls;
using System.Diagnostics;

namespace BCRFC
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        protected float timer;

        public State _currentState;
        private State _nextState;
        private UIControls ui;

        // inventory management should handle here
        private bool IsShowingPlayer = false;
     
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;

            this.Window.AllowUserResizing = false;
            this.Window.Title = "Become Corp.: Rogue Foraging Client";

            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            ui = new UIControls(this);
            this.Components.Add(ui);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, Content);
            _currentState.LoadContent();
            ui.ChangeState(_currentState);
            _nextState = null;
        }

        protected override void Update(GameTime gameTime)
        {
            {
                if (_nextState != null)
                {
                    _currentState = _nextState;
                    _currentState.LoadContent();
                    ui.ChangeState(_currentState);

                    _nextState = null;
                }

                _currentState.Update(gameTime);

                _currentState.PostUpdate(gameTime);
            }
            base.Update(gameTime);
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public void TogglePlayerForm(Player player)
        {
            if (IsShowingPlayer)
            {
                ui.HidePlayerForm();
                IsShowingPlayer = false;
            }
            else
            {
                ui.ShowPlayerForm(player, 64, 64);
                IsShowingPlayer = true;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _currentState.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
