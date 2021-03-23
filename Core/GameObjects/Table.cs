using Core;
using Core.Interfaces;
using Rogue.Actions;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Table : GameObject {
        public Table(Point location) : base(location, Color.SaddleBrown, 227, "table") {
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new NullAction();
        }
    }
}

