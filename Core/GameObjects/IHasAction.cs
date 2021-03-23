using Core;
using Core.Interfaces;

namespace Rogue.GameObjects {
    public interface IHasAction {
        IAction GetAction(IMap map, Direction from);
    }
}