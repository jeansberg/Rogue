using Rogue.GameObjects;
using Rogue.MazeGenerator;

namespace Rogue.Actions {
    public class CloseDoor : IAction {
        private readonly Door door;

        public CloseDoor(Door door) {
            this.door = door;
        }

        public void Perform(RogueMap<MapCell> map, Actor actor, bool defaultAction = false) {
            if (defaultAction) {
                return;
            }

            if(actor.Location == door.OriginalLocation) {
                return;
            }

            door.Orientation = door.OriginalOrientation;
            door.Location = door.OriginalLocation;

            door.IsOpen = false;
        }
    }
}
