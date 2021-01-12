using SadConsole;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Door : GameObject {
        public Door(Point location, Orientation orientation) : base(location, Color.SaddleBrown) {
            Orientation = orientation;
        }

        public Orientation Orientation { get; set; }
        public override string ToString() => Orientation switch {
            Orientation.Horizontal => "-",
            Orientation.Vertical => "|",
            _ => throw new System.NotImplementedException()
        };
    }
}
