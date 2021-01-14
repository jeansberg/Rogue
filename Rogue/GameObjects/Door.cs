using Rogue.Actions;
using SadConsole;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Door : GameObject {
        public Door(Point location, Orientation orientation) : base(location, Color.SaddleBrown) {
            Orientation = orientation;
            IsOpen = false;
            OriginalOrientation = orientation;
            OriginalLocation = location;
        }

        public Orientation Orientation { get; set; }
        public Orientation OriginalOrientation { get; set; }
        public Point OriginalLocation { get; set; }


        public bool IsOpen { get; set; }

        public override IAction GetAction(Direction.Types fromDirection) {
            if (IsOpen) {
                return new CloseDoor(this); 
            }

            return new OpenDoor(fromDirection, this);
        }

        public override string ToString() => Orientation switch {
            Orientation.Horizontal => "-",
            Orientation.Vertical => "|",
            _ => throw new System.NotImplementedException()
        };
    }
}
