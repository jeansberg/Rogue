using Core;
using Core.Interfaces;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Monster : Actor {
        public Monster(Point location, Color color, int glyphId, int health, string name, IFov fov) : 
            base(location, color, glyphId, health, name, fov) {
        }
    }
}
