using Core;
using Core.Interfaces;
using Rogue.Actions;

namespace Rogue.GameObjects {
    public class Chair : GameObject {
        public Chair(Point location) : base(location, System.Drawing.Color.SaddleBrown, 231, "chair") {
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new NullAction();
        }
    };
}

