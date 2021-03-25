using Core;
using Core.Interfaces;
using Rogue.Actions;

namespace Rogue.GameObjects {
    public class Door : GameObject {
        public Door(Point location, Orientation orientation) : base(location) {
            Orientation = orientation;
            IsOpen = false;
            OriginalOrientation = orientation;
            OriginalLocation = location;
        }

        public Orientation Orientation { get; set; }
        public Orientation OriginalOrientation { get; set; }
        public Point OriginalLocation { get; set; }


        public bool IsOpen { get; set; }

        public override System.Drawing.Color Color() {
            return System.Drawing.Color.SaddleBrown;
        }

        public override IAction GetAction(IMap map, Direction fromDirection) {
            if (IsOpen) {
                return new CloseDoor(this, map); 
            }

            return new OpenDoor(fromDirection, this, map);
        }

        public override int GlyphId() {
            return Orientation == Orientation.Vertical ? 179 : 196;
        }

        public override string Name() {
            return "Door";
        }
    }
}
