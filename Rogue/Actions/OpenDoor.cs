using Rogue.GameObjects;
using Rogue.MazeGenerator;
using Rogue.Services;
using SadConsole;
using SadRogue.Primitives;

namespace Rogue.Actions {
    public class OpenDoor : IAction {
        private readonly RogueMap<MapCell> map;
        private readonly Direction.Types fromDirection;
        private readonly Door door;

        public OpenDoor(Direction.Types fromDirection, Door door, RogueMap<MapCell> map) {
            this.fromDirection = fromDirection;
            this.door = door;
            this.map = map;
        }

        public ActionResult Perform(Actor actor, bool defaultAction = false) {
            var newLocation = door.Location.Increment(fromDirection.Opposite());
            if (!map.InBounds(newLocation)) {
                return ActionResult.Fail("", false);
            }

            door.Location = newLocation;

            door.Orientation = door.Orientation == Orientation.Horizontal ?
                Orientation.Vertical :
                Orientation.Horizontal;

            if (door.Orientation == SadConsole.Orientation.Vertical) {
                door.GlyphId = 179;
            }
            else {
                door.GlyphId = 196;
            }

            door.IsOpen = true;
            map[door.OriginalLocation.X, door.OriginalLocation.Y].IsTransparent = true;

            Locator.Audio.PlaySound("doorOpen");
            return ActionResult.Succeed("Opened door", true);
        }

        public override string ToString() {
            return "Opened door";
        }
    }
}
