using System;
using System.Collections.Generic;
using System.Text;

namespace BCRFC.Sprites
{
    public interface ICollidable
    {
        void OnCollide(Sprite sprite);
    }
}
