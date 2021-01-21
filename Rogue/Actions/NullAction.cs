using Rogue.GameObjects;
using Rogue.MazeGenerator;

namespace Rogue.Actions {
    public class NullAction : IAction {
        public bool Perform(RogueMap<MapCell> map, Actor actor, bool defaultAction = false) {
            return false;
        }
    }
}
