using Rogue.MazeGenerator;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Player : Actor {
        public Player(Point location, Color color, RogueSharp.FieldOfView<MapCell> fov) : base(location, color, 64, 10, "Player", fov) {
        }
    };
}

