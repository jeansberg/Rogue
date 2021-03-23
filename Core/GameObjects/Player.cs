using Core;
using Core.Interfaces;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Player : Actor {
        public Player(Point location, Color color, IFov fov) : base(location, color, 64, 10, "Player", fov) {
        }
    };
}

