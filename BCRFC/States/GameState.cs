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
using GeonBit.UI.Entities;
using GeonBit.UI;

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
        private bool IsShowingPlayer = false;
        private Panel panelPipboy = new Panel(new Vector2(540, 480), PanelSkin.Default, Anchor.TopLeft);
        private Panel panelGameUI = new Panel(new Vector2(256, 480), PanelSkin.Default, Anchor.TopRight);
        private Icon bufferItem = new Icon(IconType.None); // change to Item or something

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
            panelGameUI.Padding = Vector2.Zero;
            UserInterface.Active.AddEntity(panelGameUI);
        }

        public override void Update(GameTime gameTime)
        {
            float timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState newState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.ChangeState(new MenuState(_game, _content));
                UserInterface.Active.Clear();
            }

            if (oldState.IsKeyUp(Keys.I) && newState.IsKeyDown(Keys.I))
                TogglePlayerSheet();

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

        // check if best spot
        public void TogglePlayerSheet() // yeah change to making it visible or not
        {
            if (IsShowingPlayer)
            {
                panelPipboy.ClearChildren();
                UserInterface.Active.RemoveEntity(panelPipboy);
                IsShowingPlayer = false;
            }
            else // create screen
            {
                UserInterface.Active.AddEntity(panelPipboy);

                PanelTabs tabs = new PanelTabs();
                panelPipboy.AddChild(tabs);
                {
                    TabData tab = tabs.AddTab("Stats");
                    tab.panel.AddChild(new Paragraph("test"));
                }
                {
                    TabData tab = tabs.AddTab("Inventory");
                    //VerticalScrollbar scrollbar = new VerticalScrollbar(0, 10, Anchor.CenterRight);
                    //tab.panel.AddChild(scrollbar);
                    // left side
                    Panel panelLeft = new Panel(size: new Vector2(250, 380), skin: PanelSkin.None, anchor: Anchor.TopLeft);
                    panelLeft.Padding = Vector2.Zero;
                    tab.panel.AddChild(panelLeft);
                    panelLeft.AddChild(new Header("Player", Anchor.TopCenter));
                    // helm
                    Icon equipHelm = new Icon(IconType.None, Anchor.AutoCenter, 1, true); // maybe this all works thinks the null works
                    equipHelm.OnClick = (Entity entity) => // clean
                    {
                        Icon tempItem = new Icon(equipHelm.IconType);
                        equipHelm.IconType = bufferItem.IconType;
                        bufferItem.IconType = tempItem.IconType;
                    };
                    panelLeft.AddChild(equipHelm);
                    // chest
                    Icon equipChest = new Icon(IconType.None, Anchor.AutoCenter, 1, true); // maybe this all works thinks the null works
                    equipChest.OnClick = (Entity entity) => // clean
                    {
                        Icon tempItem = new Icon(equipChest.IconType);
                        equipChest.IconType = bufferItem.IconType;
                        bufferItem.IconType = tempItem.IconType;
                    };
                    panelLeft.AddChild(equipChest);
                    // primary
                    Icon equipPrimary = new Icon(IconType.None, Anchor.CenterLeft, 1, true); // maybe this all works thinks the null works
                    equipPrimary.OnClick = (Entity entity) => // clean
                    {
                        Icon tempItem = new Icon(equipPrimary.IconType);
                        equipPrimary.IconType = bufferItem.IconType;
                        bufferItem.IconType = tempItem.IconType;
                    };
                    panelLeft.AddChild(equipPrimary);
                    // secondary
                    Icon equipSecondary = new Icon(IconType.None, Anchor.CenterRight, 1, true); // maybe this all works thinks the null works
                    equipSecondary.OnClick = (Entity entity) => // clean
                    {
                        Icon tempItem = new Icon(equipSecondary.IconType);
                        equipSecondary.IconType = bufferItem.IconType;
                        bufferItem.IconType = tempItem.IconType;
                    };
                    panelLeft.AddChild(equipSecondary);
                    // legs
                    Icon equipLegs = new Icon(IconType.None, Anchor.AutoCenter, 1, true); // maybe this all works thinks the null works
                    equipLegs.OnClick = (Entity entity) => // clean
                    {
                        Icon tempItem = new Icon(equipLegs.IconType);
                        equipLegs.IconType = bufferItem.IconType;
                        bufferItem.IconType = tempItem.IconType;
                    };
                    panelLeft.AddChild(equipLegs);
                    // back
                    Icon equipBack = new Icon(IconType.None, Anchor.Auto, 1, true); // maybe this all works thinks the null works
                    equipBack.OnClick = (Entity entity) => // clean
                    {
                        Icon tempItem = new Icon(equipBack.IconType);
                        equipBack.IconType = bufferItem.IconType;
                        bufferItem.IconType = tempItem.IconType;
                    };
                    panelLeft.AddChild(equipBack);
                    // Ring
                    Icon equipRing = new Icon(IconType.None, Anchor.AutoInline, 1, true); // maybe this all works thinks the null works
                    equipRing.OnClick = (Entity entity) => // clean
                    {
                        Icon tempItem = new Icon(equipRing.IconType);
                        equipRing.IconType = bufferItem.IconType;
                        bufferItem.IconType = tempItem.IconType;
                    };
                    panelLeft.AddChild(equipRing);
                    // ...
                    // right side
                    Panel panelRight = new Panel(size: new Vector2(250, 380), skin: PanelSkin.None, anchor: Anchor.TopRight);
                    panelRight.Padding = Vector2.Zero;
                    tab.panel.AddChild(panelRight);
                    foreach (Inventory inv in player.Inventories)
                    {
                        panelRight.AddChild(new Header(string.Format("{0} {1} x {2}", inv.Name, inv.Width, inv.Height), Anchor.TopCenter));
                        Panel panelInv = new Panel(size: new Vector2(inv.Width * 52, inv.Height * 52), skin: PanelSkin.Simple, anchor: Anchor.AutoCenter);
                        panelInv.Padding = Vector2.Zero;
                        panelRight.AddChild(panelInv);
                        bool tempChange = false;
                        for (int i = 0; i < inv.Width; i++) // i think i and j are mixed
                        {
                            for (int j = 0; j < inv.Height; j++)
                            {
                                Icon item; // just testing :P
                                if (tempChange)
                                    item = new Icon(IconType.Sword, Anchor.AutoInline, 1); // setting true after makes border
                                else
                                    item = new Icon(IconType.Apple, Anchor.AutoInline, 1);
                                tempChange = !tempChange;
                                item.Padding = Vector2.Zero;
                                item.ToolTipText = "test";
                                item.OnClick = (Entity entity) => // check if holding item and place in buffer item
                                {
                                    Icon tempItem = new Icon(item.IconType);
                                    item.IconType = bufferItem.IconType;
                                    bufferItem.IconType = tempItem.IconType;
                                };
                                panelInv.AddChild(item);
                            }
                        }
                    }
                    //scrollbar.OnValueChange = (Entity entity) =>
                    //{

                    //};
                }

                IsShowingPlayer = true;
            }
        }
    }
}