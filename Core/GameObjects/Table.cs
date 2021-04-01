using Core;
using Core.Interfaces;
using Rogue.Actions;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Table : GameObject {
        public Table(Point location) : base(location) {
        }

        public override Color Color() {
            return System.Drawing.Color.SaddleBrown;
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new NullAction();
        }

        public override int GlyphId() {
            return 227;
        }

        public override string Name() {
            return "Table";        }

        public override void Update() {
        }
    }
}

