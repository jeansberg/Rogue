using RogueSharp;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Actor : GameObject {
        public Actor(SadRogue.Primitives.Point location, Color color, int glyphId) : base(location, color, glyphId) {
        }

        public override IAction GetAction(Direction.Types from) {
            throw new System.NotImplementedException();
        }
    }
}
