using Core;
using Core.Interfaces;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public abstract class GameObject : IHasAction {
        public GameObject(Point location, Color color, int glyphId, string name, GameObjectType type = GameObjectType.Uncategorized) {
            Location = location;
            Color = color;
            GlyphId = glyphId;
            Name = name;
        }

        public Color Color { get; set; }
        public int GlyphId { get; set; }
        public Point Location { get; set; }
        public string Name { get; set; }
        public GameObjectType Type { get; set; }
        public abstract IAction GetAction(IMap map, Direction from);
    }
}
