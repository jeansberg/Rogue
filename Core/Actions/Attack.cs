using Rogue.GameObjects;
using Rogue.Services;

namespace Rogue.Actions {
    public class Attack : IAction {
        private readonly Actor target;
        private readonly Missile? missile;

        public Attack(Actor target, Missile? missile) {
            this.target = target;
            this.missile = missile;
        }

        public ActionResult Perform(Actor actor, bool defaultAction = false) {
            Locator.Audio.PlaySound("attack", actor.Name());
            
            var damage = new DamageRange(target, actor, missile).GetDamage();
            target.TakeDamage(damage);

            if (!target.IsAlive && actor is Player p) {
                p.SetExperience(p.GetExperience() + Monster.ExperienceReward(((Monster)target).MonsterType));
            }

            return ActionResult.Succeed($"{actor.Name()} attacked {target.Name()} for {damage} damage", false);
        }
    }
}
