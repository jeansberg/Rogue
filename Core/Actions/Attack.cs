using Rogue.GameObjects;
using Rogue.Services;

namespace Rogue.Actions {
    public class Attack : IAction {
        private readonly Actor target;

        public Attack(Actor target) {
            this.target = target;
        }

        public ActionResult Perform(Actor actor, bool defaultAction = false) {
            var damage = actor.Weapon == null ? 1 : 2;
            target.Health -= damage;

            Locator.Audio.PlaySound("hit");
            return ActionResult.Succeed($"{actor.Name} attacked {target.Name} for {damage} damage", false);
        }
    }
}
