using SadRogue.Primitives;

namespace Rogue.Map {
    public class Room {
        public Room(Rectangle bounds) {
            Bounds = bounds;
        }

        public Rectangle Bounds { get; private set; }
    }
}
