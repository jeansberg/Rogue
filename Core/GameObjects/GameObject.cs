using Core;
using Core.Interfaces;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public abstract class GameObject : IHasAction {
        public GameObject(Point location, GameObjectType type = GameObjectType.Uncategorized) {
            Location = location;
        }

        public abstract Color Color();
        public abstract int GlyphId();
        public abstract string Name();

        public Point Location { get; set; }
        public GameObjectType Type { get; set; }
        public abstract IAction GetAction(IMap map, Direction from);
    }
}
