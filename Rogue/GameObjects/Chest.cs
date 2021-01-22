using Rogue.Actions;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Chest : GameObject {
        public Chest(Point location) : base(location, Color.SaddleBrown, 205, "chest") {
        }

        public override IAction GetAction(RogueMap<MazeGenerator.MapCell> map, Direction.Types from) {
            return new NullAction();
        }
    }
}
