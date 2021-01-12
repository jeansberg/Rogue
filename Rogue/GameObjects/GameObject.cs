using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class GameObject {
        public GameObject(Point location, Color color) {
            Location = location;
            Color = color;
        }

        public Color Color { get; set; }
        public Point Location { get; set; }
    }
}
