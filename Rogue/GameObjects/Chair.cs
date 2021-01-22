using Rogue.Actions;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Chair : GameObject {
        public Chair(Point location) : base(location, Color.SaddleBrown, 231, "chair") {
        }

        public override IAction GetAction(RogueMap<MazeGenerator.MapCell> map, Direction.Types from) {
            return new NullAction();
        }
    };
}

