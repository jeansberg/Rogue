using Rogue.GameObjects;

namespace Rogue.Actions {
    public class Attack : IAction {
        private readonly Actor target;

        public Attack(Actor target) {
            this.target = target;
        }

        public ActionResult Perform(Actor actor, bool defaultAction = false) {
            target.Health--;

            return ActionResult.Succeed($"{actor.Name} attacked {target.Name}", false);
        }
    }
}
