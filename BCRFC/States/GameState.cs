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
        private Item tempItem;
        private Item buffered;

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
                    Item itemHelm = player.equipped[0];
                    Image equipHelm;
                    if (itemHelm != null)
                    {
                        equipHelm = new Image(_content.Load<Texture2D>("Sprites/Items/" + itemHelm.Name), new Vector2(64, 64), anchor: Anchor.AutoCenter);
                        equipHelm.ToolTipText = string.Format("{0}:\n{1}", itemHelm.Name, itemHelm.Description);
                    }
                    else
                    {
                        equipHelm = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoCenter);
                    }
                    equipHelm.OnClick = (Entity entity) => // clean
                    {
                        if (equipHelm.TextureName != "Sprites/Items/Air" || (bufferItem.TextureName != "Sprites/Items/Air" && buffered.Type == "Helmet")) // find better way
                        {
                            Image tempItemShown = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoInline); // maybe be kbuffer myabe redundant
                            tempItemShown.Texture = equipHelm.Texture;
                            tempItemShown.ToolTipText = equipHelm.ToolTipText;
                            equipHelm.Texture = bufferItem.Texture;
                            equipHelm.ToolTipText = bufferItem.ToolTipText;
                            bufferItem.Texture = tempItemShown.Texture; // needs to be a shallow clone i think
                            bufferItem.ToolTipText = tempItemShown.ToolTipText;
                            tempItem = buffered;
                            buffered = player.SwapEquipped(tempItem, 0);
                            bufferItem.RemoveFromParent();
                            if (bufferItem.TextureName != "Sprites/Items/Air") // if buffer item is something and check if helmet item
                            {
                                UserInterface.Active.AddEntity(bufferItem);
                                bufferItem.Anchor = Anchor.TopLeft;
                                bufferItem.BeforeDraw = (Entity entity1) =>
                                {
                                    entity1.Offset = new Vector2(Mouse.GetState().X + 8, Mouse.GetState().Y + 8);
                                };
                            }
                        }
                    };
                    panelLeft.AddChild(equipHelm);
                    // chest
                    Item itemChest = player.equipped[1];
                    Image equipChest; // maybe this all works thinks the null works
                    if (itemChest != null)
                    {
                        equipChest = new Image(_content.Load<Texture2D>("Sprites/Items/" + itemChest.Name), new Vector2(64, 64), anchor: Anchor.AutoCenter);
                        equipChest.ToolTipText = string.Format("{0}:\n{1}", itemChest.Name, itemChest.Description);
                    }
                    else
                    {
                        equipChest = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoCenter);
                    }
                    equipChest.OnClick = (Entity entity) => // clean
                    {
                        if (equipChest.TextureName != "Sprites/Items/Air" || (bufferItem.TextureName != "Sprites/Items/Air" && buffered.Type == "Chest")) // find better way
                        {
                            Image tempItemShown = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoInline); // maybe be kbuffer myabe redundant
                            tempItemShown.Texture = equipChest.Texture;
                            tempItemShown.ToolTipText = equipChest.ToolTipText;
                            equipChest.Texture = bufferItem.Texture;
                            equipChest.ToolTipText = bufferItem.ToolTipText;
                            bufferItem.Texture = tempItemShown.Texture; // needs to be a shallow clone i think
                            bufferItem.ToolTipText = tempItemShown.ToolTipText;
                            tempItem = buffered;
                            buffered = player.SwapEquipped(tempItem, 1);
                            bufferItem.RemoveFromParent();
                            if (bufferItem.TextureName != "Sprites/Items/Air") // if buffer item is something and check if helmet item
                            {
                                UserInterface.Active.AddEntity(bufferItem);
                                bufferItem.Anchor = Anchor.TopLeft;
                                bufferItem.BeforeDraw = (Entity entity1) =>
                                {
                                    entity1.Offset = new Vector2(Mouse.GetState().X + 8, Mouse.GetState().Y + 8);
                                };
                            }
                        }
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
                    Item itemBack = player.equipped[5];
                    Image equipBack; // maybe this all works thinks the null works
                    if (itemBack != null)
                    {
                        equipBack = new Image(_content.Load<Texture2D>("Sprites/Items/" + itemBack.Name), new Vector2(64, 64), anchor: Anchor.Auto);
                        equipBack.ToolTipText = string.Format("{0}:\n{1}", itemBack.Name, itemBack.Description);
                    }
                    else
                    {
                        equipBack = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.Auto);
                    }
                    equipBack.OnClick = (Entity entity) => // clean
                    {
                        if (equipBack.TextureName != "Sprites/Items/Air" || (bufferItem.TextureName != "Sprites/Items/Air" && buffered.Type == "Back")) // find better way
                        {
                            Image tempItemShown = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoInline); // maybe be kbuffer myabe redundant
                            tempItemShown.Texture = equipBack.Texture;
                            tempItemShown.ToolTipText = equipBack.ToolTipText;
                            equipBack.Texture = bufferItem.Texture;
                            equipBack.ToolTipText = bufferItem.ToolTipText;
                            bufferItem.Texture = tempItemShown.Texture; // needs to be a shallow clone i think
                            bufferItem.ToolTipText = tempItemShown.ToolTipText;
                            tempItem = buffered;
                            buffered = player.SwapEquipped(tempItem, 5);
                            bufferItem.RemoveFromParent();
                            if (bufferItem.TextureName != "Sprites/Items/Air") // if buffer item is something and check if helmet item
                            {
                                UserInterface.Active.AddEntity(bufferItem);
                                bufferItem.Anchor = Anchor.TopLeft;
                                bufferItem.BeforeDraw = (Entity entity1) =>
                                {
                                    entity1.Offset = new Vector2(Mouse.GetState().X + 8, Mouse.GetState().Y + 8);
                                };
                            }
                        }
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
                                    if (itemShown.TextureName != "Sprites/Items/Air" || bufferItem.TextureName != "Sprites/Items/Air") // find better way
                                    {
                                        Image tempItemShown = new Image(_content.Load<Texture2D>("Sprites/Items/Air"), new Vector2(64, 64), anchor: Anchor.AutoInline); // maybe be kbuffer myabe redundant
                                        tempItemShown.Texture = itemShown.Texture;
                                        tempItemShown.ToolTipText = itemShown.ToolTipText;
                                        itemShown.Texture = bufferItem.Texture;
                                        itemShown.ToolTipText = bufferItem.ToolTipText;
                                        bufferItem.Texture = tempItemShown.Texture; // needs to be a shallow clone i think
                                        bufferItem.ToolTipText = tempItemShown.ToolTipText;
                                        int x = entity.Parent._children.IndexOf(entity) % inv.Width;
                                        int y = entity.Parent._children.IndexOf(entity) % inv.Height;
                                        Debug.WriteLine(x + " " + y); // how does this work its like magic also check on bigger invs cuase this math is wierd
                                        //tempItem = inv.GetItem(x, y); // deal with inv swap as well
                                        //item = inv.GetItem(x, y);
                                        //buffered = inv.GetItem(x, y); // move outside i think
                                        tempItem = buffered;
                                        Debug.WriteLine("Held:" + tempItem + " -> " + x + ", " + y + " Inventory: " + buffered); // i think this is wrong
                                        buffered = inv.SwapItems(tempItem, x, y);
                                        Debug.WriteLine("Inventory:" + tempItem + " -> " + x + ", " + y + " Held: " + buffered);
                                        //Debug.WriteLine(inv.Name + inv.GetItem(0, 0).Name);
                                        // follow mouse needs work
                                        bufferItem.RemoveFromParent();
                                        if (bufferItem.TextureName != "Sprites/Items/Air") // if buffer item is something
                                        {
                                            UserInterface.Active.AddEntity(bufferItem);
                                            bufferItem.Anchor = Anchor.TopLeft;
                                            bufferItem.BeforeDraw = (Entity entity1) =>
                                            {
                                                entity1.Offset = new Vector2(Mouse.GetState().X + 8, Mouse.GetState().Y + 8);
                                            };
                                        }
                                    }
                                };
                                itemShown.OnRightClick = (Entity entity) =>
                                {
                                    if (itemShown.TextureName != "Sprites/Items/Air")
                                    {
                                        entity.ClearChildren();
                                        entity.Parent.Children.ToList().ForEach(delegate(Entity entity1) { entity1.ClearChildren(); }); // this works im magic
                                        Panel panelOptions = new Panel(size: new Vector2(156, 192), skin: PanelSkin.Golden, anchor: Anchor.Auto, offset: new Vector2(52, 52));
                                        // use button change layout
                                        Button use = new Button("use", ButtonSkin.Alternative, Anchor.TopCenter, new Vector2(128, 32));
                                        use.OnClick = (Entity entity) => { };
                                        // mod
                                        Button mod = new Button("mod", ButtonSkin.Alternative, Anchor.AutoCenter, new Vector2(128, 32));
                                        mod.OnClick = (Entity entity) => { };
                                        // discard
                                        Button discard = new Button("discard", ButtonSkin.Alternative, Anchor.AutoCenter, new Vector2(128, 32));
                                        discard.OnClick = (Entity entity) => { };
                                        // appearance
                                        Button appear = new Button("appear", ButtonSkin.Alternative, Anchor.AutoCenter, new Vector2(128, 32));
                                        appear.OnClick = (Entity entity) => { };
                                        panelOptions.AddChild(use).Padding = Vector2.Zero;
                                        panelOptions.AddChild(mod).Padding = Vector2.Zero;
                                        panelOptions.AddChild(discard).Padding = Vector2.Zero;
                                        panelOptions.AddChild(appear).Padding = Vector2.Zero;
                                        itemShown.AddChild(panelOptions);
                                        itemShown.PriorityBonus = -1; // find way
                                        entity.Parent.OnClick = (Entity entity) => { entity.Children.ToList().ForEach(delegate (Entity entity1) { entity1.ClearChildren(); }); }; // needs one more parent but good enough
                                    }
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