using Core;
using Core.Interfaces;
using Rogue.GameObjects;
using Rogue.Services;

namespace Rogue.Actions {
    public class CloseDoor : IAction {
        private readonly IMap map;
        private readonly Door door;

        public CloseDoor(Door door, IMap map) {
            this.door = door;
            this.map = map;
        }

        public ActionResult Perform(Actor actor, bool defaultAction = false) {
            if (defaultAction || !door.IsOpen) {
                return ActionResult.Cancel("", true);
            }

            if (actor.Location.Equals(door.OriginalLocation)) {
                return ActionResult.Fail("That might hurt", false);
            }

            door.Orientation = door.OriginalOrientation;

            door.Location = door.OriginalLocation;

            door.IsOpen = false;
            map.SetTransparent(door.OriginalLocation, false);

            Locator.Audio.PlaySound("doorClose");
            return ActionResult.Succeed("Closed door", false);
        }
    }
}
