using Rogue.Actions;
using Rogue.GameObjects;
using Rogue.MazeGenerator;

namespace Rogue {
    public interface IAction {
        ActionResult Perform(Actor actor, bool defaultAction = false);
    }
}
