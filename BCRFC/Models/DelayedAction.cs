using System;
using System.Collections.Generic;
using System.Text;

namespace BCRFC.Models
{
    class DelayedAction
    {
        public Action Action { get; private set; }
        public float Delay { get; private set; }
        public float TimeRemaining { get; private set; }

        public DelayedAction(Action action, float delay)
        {
            TimeRemaining = delay;
            Action = action;
            Delay = delay;
        }

        public bool Update(float deltaTime)
        {
            TimeRemaining -= deltaTime;

            if (TimeRemaining <= 0)
            {
                Action();
                return false;
            }

            return true;
        }
    }
}
