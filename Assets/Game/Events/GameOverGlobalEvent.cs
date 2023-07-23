using ME.ECS;

namespace Game.Events
{
    public class GameOverGlobalEvent : GlobalEvent
    {
        public float delay;

        public void Execute(float delay)
        {
            this.delay = delay;
            base.Execute();
        }
    }
}