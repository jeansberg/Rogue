using Rogue.MazeGenerator;

namespace Rogue {
    public interface IAction {
        void Perform(RogueMap<MapCell> map, bool defaultAction = false);
    }
}
