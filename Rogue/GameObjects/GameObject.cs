using SadRogue.Primitives;

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

        public abstract IAction GetAction(RogueMap<MazeGenerator.MapCell> map, Direction.Types from);
    }
}
