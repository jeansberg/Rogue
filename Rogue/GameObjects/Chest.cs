using Rogue.Actions;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Chest : GameObject {
        public Chest(Point location) : base(location, Color.SaddleBrown, 205) {
        }

        public override IAction GetAction(Direction.Types from) {
            return new NullAction();
        }
    }
}
