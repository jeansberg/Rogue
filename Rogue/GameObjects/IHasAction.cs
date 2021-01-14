using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public interface IHasAction {
        IAction GetAction(Direction.Types from);
    }
}