using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public abstract class GameObject : IHasAction {
        public GameObject(Point location, Color color, int glyphId) {
            Location = location;
            Color = color;
            GlyphId = glyphId;
        }

        public Color Color { get; set; }
        public int GlyphId { get; set; }
        public Point Location { get; set; }

        public abstract IAction GetAction(Direction.Types from);
    }
}
