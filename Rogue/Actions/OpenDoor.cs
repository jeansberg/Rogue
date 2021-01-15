using Rogue.GameObjects;
using Rogue.MazeGenerator;
using SadConsole;
using SadRogue.Primitives;

namespace Rogue.Actions {
    public class OpenDoor : IAction {
        private readonly Direction.Types fromDirection;
        private readonly Door door;

        public OpenDoor(Direction.Types fromDirection, Door door) {
            this.fromDirection = fromDirection;
            this.door = door;
        }

        public void Perform(RogueMap<MapCell> map, Actor actor, bool defaultAction = false) {
            var newLocation = door.Location.Increment(fromDirection.Opposite());
            if (map.InBounds(newLocation)) {
                door.Location = newLocation;

                door.Orientation = door.Orientation == Orientation.Horizontal ?
                    Orientation.Vertical :
                    Orientation.Horizontal;

                door.IsOpen = true;
            }
        }
    }
}
