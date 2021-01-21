using Rogue.Actions;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Chair : GameObject {
        public Chair(Point location) : base(location, Color.SaddleBrown, 231) {
        }

        public override IAction GetAction(Direction.Types from) {
            return new NullAction();
        }
    };
}

