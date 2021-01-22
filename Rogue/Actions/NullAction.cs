using Rogue.GameObjects;
using Rogue.MazeGenerator;

namespace Rogue.Actions {
    public class NullAction : IAction {
        public ActionResult Perform(Actor actor, bool defaultAction = false) {
            return ActionResult.Cancel("", true);
        }
    }
}
