using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCRFC.Sprites
{
    class Pickup : Sprite
    {
        public String Name;
        public String Desc; // all data of item

        public Pickup(Texture2D texture) : base(texture)
        {
            Position = new Vector2(120, 40);
            Name = "Place Holder";
            Desc = "Place Holder"; // have this create the item from here and be sneaky rather than leading to an item
        }
    }
}
