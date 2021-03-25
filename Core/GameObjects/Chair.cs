using Core;
using Core.Interfaces;
using Rogue.Actions;

namespace Rogue.GameObjects {
    public class Chair : GameObject {
        public Chair(Point location) : base(location) {
        }

        public override System.Drawing.Color Color() {
            return System.Drawing.Color.SaddleBrown;
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new NullAction();
        }

        public override int GlyphId() {
            return 231;
        }

        public override string Name() {
            return "Chair";
        }
    };
}

