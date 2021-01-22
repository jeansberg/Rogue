using Rogue.Actions;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Table : GameObject {
        public Table(Point location) : base(location, Color.SaddleBrown, 227, "table") {
        }

        public override IAction GetAction(RogueMap<MazeGenerator.MapCell> map, Direction.Types from) {
            return new NullAction();
        }
    }
}

