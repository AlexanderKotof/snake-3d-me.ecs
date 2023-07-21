using ME.ECS;

namespace Game.Features.GameField.Components {

    public struct GameFieldSize : IComponent
    {
        public int xSize;
        public int ySize;
    }

    public struct IsGameField : IComponent
    {
    }
}