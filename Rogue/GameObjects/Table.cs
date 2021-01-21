using Rogue.Actions;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Table : GameObject {
        public Table(Point location) : base(location, Color.SaddleBrown, 227) {
        }

        public override IAction GetAction(Direction.Types from) {
            return new NullAction();
        }
    }
}

