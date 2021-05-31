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
using System.Diagnostics;

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
        private Image bufferItem; // change to Item or something

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
            bufferItem = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoInline);
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
                    Image equipHelm = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoCenter); // maybe this all works thinks the null works
                    equipHelm.OnClick = (Entity entity) => // clean
                    {
                        Image tempItem = equipHelm;
                        equipHelm = bufferItem;
                        bufferItem = tempItem;
                    };
                    panelLeft.AddChild(equipHelm);
                    // chest
                    Image equipChest = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoCenter); // maybe this all works thinks the null works
                    equipChest.OnClick = (Entity entity) => // clean
                    {
                        Image tempItem = equipChest;
                        equipChest = bufferItem;
                        bufferItem = tempItem;
                    };
                    panelLeft.AddChild(equipChest);
                    // primary
                    Image equipPrimary = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.CenterLeft); // maybe this all works thinks the null works
                    equipPrimary.OnClick = (Entity entity) => // clean
                    {
                        Image tempItem = equipPrimary;
                        equipPrimary = bufferItem;
                        bufferItem = tempItem;
                    };
                    panelLeft.AddChild(equipPrimary);
                    // secondary
                    Image equipSecondary = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.CenterRight); // maybe this all works thinks the null works
                    equipSecondary.OnClick = (Entity entity) => // clean
                    {
                        Image tempItem = equipSecondary;
                        equipSecondary = bufferItem;
                        bufferItem = tempItem;
                    };
                    panelLeft.AddChild(equipSecondary);
                    // legs
                    Image equipLegs = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoCenter); // maybe this all works thinks the null works
                    equipLegs.OnClick = (Entity entity) => // clean
                    {
                        Image tempItem = equipLegs;
                        equipLegs = bufferItem;
                        bufferItem = tempItem;
                    };
                    panelLeft.AddChild(equipLegs);
                    // back
                    Image equipBack = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.Auto); // maybe this all works thinks the null works
                    equipBack.OnClick = (Entity entity) => // clean
                    {
                        Image tempItem = equipBack;
                        equipBack = bufferItem;
                        bufferItem = tempItem;
                    };
                    panelLeft.AddChild(equipBack);
                    // Ring
                    Image equipRing = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoInline); // maybe this all works thinks the null works
                    equipRing.OnClick = (Entity entity) => // clean
                    {
                        Image tempItem = equipRing;
                        equipRing = bufferItem;
                        bufferItem = tempItem;
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
                        Panel panelInv = new Panel(size: new Vector2(inv.Width * 72, inv.Height * 72), skin: PanelSkin.Simple, anchor: Anchor.AutoCenter);
                        panelInv.Padding = Vector2.Zero;
                        panelRight.AddChild(panelInv);
                        for (int i = 0; i < inv.Width; i++) // i think i and j are mixed
                        {
                            for (int j = 0; j < inv.Height; j++)
                            {
                                Item item = inv.GetItem(i, j); // just testing :P
                                Image itemShown;
                                if (item != null)
                                {
                                    itemShown = new Image(_content.Load<Texture2D>("Sprites/Items/" + item.Name), new Vector2(64, 64), anchor: Anchor.AutoInline);
                                    itemShown.ToolTipText = string.Format("{0}:\n{1}", item.Name, item.Description);
                                } else
                                {
                                    itemShown = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoInline);
                                }
                                itemShown.Padding = Vector2.Zero;
                                itemShown.OnClick = (Entity entity) => // check if holding item and place in buffer item
                                {
                                    Image tempItemShown = itemShown;
                                    itemShown = bufferItem;
                                    bufferItem = tempItemShown;
                                };
                                panelInv.AddChild(itemShown);
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