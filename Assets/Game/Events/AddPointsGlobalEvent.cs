using ME.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class AddPointsGlobalEvent : GlobalEvent
    {
        public int addApples;
        public int snakeLength;

        public void Execute(int addApples, int snakeLength)
        {
            this.addApples = addApples;
            this.snakeLength = snakeLength;
            Execute();
        }
    }
}