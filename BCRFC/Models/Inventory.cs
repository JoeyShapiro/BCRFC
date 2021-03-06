using System;
using System.Collections.Generic;
using System.Text;

namespace BCRFC.Models
{
    public class Inventory // check if correct folder for this
    {
        public int Width;
        public int Height;
        public string Name;

        // find best way either index or index array or array for allocation or id in item or here or index that changes or checking upon adding and either local id or setting here somehow
        private Item[,] Items;

        public Inventory()
        {
            Width = 1;
            Height = 1;
            Items = new Item[Width, Height];
        }

        public Inventory(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
            Items = new Item[Width, Height];
        }

        public void AddItem(Item item) // maybe keep or use other one
        {

        }

        public bool TryAddItem(Item item, int x, int y)
        {
            // loop through each element of array to check entire item fits in array
            if (Items[x, y] != null)
                return false;
            Items[x, y] = item;

            return true;
        }

        public void RemoveItem(Item item)
        {

        }

        public Item GetItem(int x, int y)
        {
            return Items[x, y];
        }

        public Item SwapItems(Item item, int x, int y)
        {
            // add checks for bigger items later
            Item bufferItem = Items[x, y];
            Items[x, y] = item;
            return bufferItem;
        }

        public void ShowItems() // maybe list then pass to player to ListInventory()
        {

        }
    }
}
