using GeonBit.UI.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCRFC.Models
{
    public class Item
    {
        int Width;
        int Height;
        int Name;
        // maybe remove these
        int GID; // gloabl id
        int LID; // local id
        // maybe grid of 1s to determine size and shape of space rather than 2D
        // image here
        Image image;
        string Type;
        // find out how to deal with syblings

        public Item()
        {
            Width = 1;
            Height = 1;
        }
    }
}
