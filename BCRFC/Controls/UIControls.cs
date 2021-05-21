using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.UI.Forms;
using BCRFC.States;

namespace BCRFC.Controls
{
    class UIControls : ControlManager
    {
        Game1 _game;
        private static int screenWidth;
        private static int screenHeight;

        public UIControls(Game1 game) : base(game)
        {
            _game = game;
            // find better way of getting
            screenWidth = 1280;
            screenHeight = 720;
        }

        public override void InitializeComponent()
        {
            
        }

        private void Btn1_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundColor = Color.DarkBlue;
        }

        private void BtnStart_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            _game.ChangeState(new GameState(_game, _game.Content));
        }

        private void BtnCodex_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            // see if good way
            Controls.RemoveAll(delegate (Control c) { return true; });
            LoadCodex();
        }
        private void BtnOptions_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Controls.RemoveAll(delegate (Control c) { return true; });
            LoadOptions();
        }

        private void BtnExit_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            _game.Exit();
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Controls.RemoveAll(delegate (Control c) { return true; });
            LoadMenu();
        }

        public void ChangeState(State state)
        {
            Controls.RemoveAll(delegate (Control c) { return true; });

            if (state.GetType() == typeof(MenuState))
                LoadMenu();
            else if (state.GetType() == typeof(GameState))
                LoadGame();
        }

        public void LoadMenu()
        {
            // make BIGGER
            var lblTitle = new Label()
            {
                Text = "Become Corp.: Rogue Foraging Client",
                Size = new Vector2(256, 64),
                TextColor = Color.BlueViolet,
                Location = new Vector2(screenWidth / 8, screenHeight / 8)
            };
            // find better way to dynamically update maybe with foreach for all and List
            var btnStart = new Button()
            {
                Text = "Start",
                Size = new Vector2(128, 32),
                BackgroundColor = Color.Blue,
                Location = new Vector2(screenWidth / 4, screenHeight / 4)
            };
            float pX = btnStart.Location.X;
            float pY = btnStart.Location.Y;
            var btnCodex = new Button()
            {
                Text = "Codex",
                Size = new Vector2(128, 32),
                BackgroundColor = Color.Blue,
                Location = new Vector2(pX, pY + 48)
            };
            pY = btnCodex.Location.Y;
            var btnOptions = new Button()
            {
                Text = "Options",
                Size = new Vector2(128, 32),
                BackgroundColor = Color.Blue,
                Location = new Vector2(pX, pY + 48)
            };
            pY = btnOptions.Location.Y;
            var btnExit = new Button()
            {
                Text = "Exit",
                Size = new Vector2(128, 32),
                BackgroundColor = Color.Blue,
                Location = new Vector2(pX, pY + 48)
            };

            btnStart.Clicked += BtnStart_Clicked;
            btnCodex.Clicked += BtnCodex_Clicked;
            btnOptions.Clicked += BtnOptions_Clicked;
            btnExit.Clicked += BtnExit_Clicked;
            Controls.Add(lblTitle);
            Controls.Add(btnStart);
            Controls.Add(btnCodex);
            Controls.Add(btnOptions);
            Controls.Add(btnExit);
        }

        public void LoadGame()
        {
            var btn1 = new Button()
            {
                Text = "1",
                Size = new Vector2(32, 32),
                BackgroundColor = Color.Blue,
                Location = new Vector2(640, 320)
            };

            btn1.Clicked += Btn1_Clicked;
            Controls.Add(btn1);
        }

        public void LoadCodex()
        {
            var btnBack = new Button()
            {
                Text = "Back",
                Size = new Vector2(128, 32),
                BackgroundColor = Color.Blue,
                Location = new Vector2(0, 0)
            };
            btnBack.Clicked += BtnBack_Clicked;
            Controls.Add(btnBack);
        }

        public void LoadOptions()
        {
            var btnBack = new Button()
            {
                Text = "Back",
                Size = new Vector2(128, 32),
                BackgroundColor = Color.Blue,
                Location = new Vector2(0, 0)
            };
            btnBack.Clicked += BtnBack_Clicked;
            Controls.Add(btnBack);
        }
    }
}
