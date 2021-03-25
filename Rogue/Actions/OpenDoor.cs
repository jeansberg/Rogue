using Core;
using Core.Interfaces;
using Rogue.GameObjects;
using Rogue.Services;

namespace Rogue.Actions {
    public class OpenDoor : IAction {
        private readonly IMap map;
        private readonly Direction fromDirection;
        private readonly Door door;

        public OpenDoor(Direction fromDirection, Door door, IMap map) {
            this.fromDirection = fromDirection;
            this.door = door;
            this.map = map;
        }

        public ActionResult Perform(Actor actor, bool defaultAction = false) {
            var newLocation = door.Location.Increment(fromDirection.Opposite());
            if (!map.IsInBounds(newLocation)) {
                return ActionResult.Fail("", false);
            }

            door.Location = newLocation;

            door.Orientation = door.Orientation == Orientation.Horizontal ?
                Orientation.Vertical :
                Orientation.Horizontal;

            door.IsOpen = true;
            map.SetTransparent(door.OriginalLocation, true);

            Locator.Audio.PlaySound("doorOpen");
            return ActionResult.Succeed("Opened door", true);
        }

        public override string ToString() {
            return "Opened door";
        }
    }
}
