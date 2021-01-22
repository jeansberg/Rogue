using Rogue.Actions;
using Rogue.MazeGenerator;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Monster : Actor {
        public Monster(Point location, Color color, int glyphId, int health, string name, RogueSharp.FieldOfView<MapCell> fov) : 
            base(location, color, glyphId, health, name, fov) {
        }
    }
}
