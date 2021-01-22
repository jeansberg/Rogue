using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public interface IHasAction {
        IAction GetAction(RogueMap<MazeGenerator.MapCell> map, Direction.Types from);
    }
}