using Core;
using Core.Interfaces;
using Rogue.Actions;

namespace Rogue.GameObjects {
    public class Chest : GameObject {
        public Chest(Point location) : base(location) {
        }

        public override System.Drawing.Color Color() {
            return System.Drawing.Color.SaddleBrown;
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new NullAction();
        }

        public override int GlyphId() {
            return 205;
        }

        public override string Name() {
            return "Chair";
        }
    }
}
