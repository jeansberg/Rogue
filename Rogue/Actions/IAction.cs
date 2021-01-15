using Rogue.GameObjects;
using Rogue.MazeGenerator;

namespace Rogue {
    public interface IAction {
        void Perform(RogueMap<MapCell> map, Actor actor, bool defaultAction = false);
    }
}
