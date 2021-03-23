using Core;
using Core.Interfaces;
using Rogue.Actions;

namespace Rogue.GameObjects {
    public class Chest : GameObject {
        public Chest(Point location) : base(location, System.Drawing.Color.SaddleBrown, 205, "chest") {
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new NullAction();
        }
    }
}
