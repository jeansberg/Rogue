using Rogue.GameObjects;
using Rogue.MazeGenerator;

namespace Rogue {
    public interface IAction {
        bool Perform(RogueMap<MapCell> map, Actor actor, bool defaultAction = false);
    }
}
