using Rogue.Actions;
using Rogue.GameObjects;

namespace Rogue {
    public interface IAction {
        ActionResult Perform(Actor actor, bool defaultAction = false);
    }
}
