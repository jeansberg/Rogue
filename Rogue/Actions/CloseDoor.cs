using Rogue.GameObjects;
using Rogue.MazeGenerator;

namespace Rogue.Actions {
    public class CloseDoor : IAction {
        private Door door;

        public CloseDoor(Door door) {
            this.door = door;
        }

        public void Perform(RogueMap<MapCell> map, bool defaultAction = false) {
            if (defaultAction) {
                return;
            }

            door.Orientation = door.OriginalOrientation;
            door.Location = door.OriginalLocation;

            door.IsOpen = false;

            //TODO Actor should not be able to close door on self
        }
    }
}
