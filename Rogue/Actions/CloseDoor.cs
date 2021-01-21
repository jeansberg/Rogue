using Rogue.GameObjects;
using Rogue.MazeGenerator;

namespace Rogue.Actions {
    public class CloseDoor : IAction {
        private readonly Door door;

        public CloseDoor(Door door) {
            this.door = door;
        }

        public bool Perform(RogueMap<MapCell> map, Actor actor, bool defaultAction = false) {
            if (defaultAction || !door.IsOpen) {
                return false;
            }

            if(actor.Location == door.OriginalLocation) {
                return false;
            }

            door.Orientation = door.OriginalOrientation;
            if (door.Orientation == SadConsole.Orientation.Vertical) {
                door.GlyphId = 179;
            }
            else {
                door.GlyphId = 196;
            }

            door.Location = door.OriginalLocation;

            door.IsOpen = false;
            map[door.OriginalLocation.X, door.OriginalLocation.Y].IsTransparent = false;

            return true;
        }

        public override string ToString() {
            return "Closed door";
        }
    }
}
