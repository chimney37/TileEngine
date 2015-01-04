using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicTile
{
    public abstract class GameObserver
    {
        public abstract void OnNotify(GameCore gCore);
    }
}
