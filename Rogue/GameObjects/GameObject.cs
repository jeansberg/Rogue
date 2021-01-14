using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public abstract class GameObject : IHasAction {
        public GameObject(Point location, Color color) {
            Location = location;
            Color = color;
        }

        public Color Color { get; set; }
        public Point Location { get; set; }

        public abstract IAction GetAction(Direction.Types from);
    }
}
