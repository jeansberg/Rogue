using Core;
using Core.Interfaces;
using Rogue.Actions;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Missile : GameObject {
        public bool Moving { get; set; }
        public Missile(Point location) : base(location) {
            Moving = true;
        }

        public Orientation Orientation { get; set; }

        public override Color Color() {
            return System.Drawing.Color.SaddleBrown;
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new NullAction();
        }

        public override int GlyphId() {
            return Orientation == Orientation.Horizontal ? 196 : 179;
        }

        public override string Name() {
            return "Missile";
        }

        public override void Update() {
        }
    }
}
