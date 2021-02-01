using Rogue.GameObjects;
using Rogue.MazeGenerator;
using Rogue.Services;

namespace Rogue.Actions {
    public class PickUp : IAction {
        private readonly GameObject item;
        private readonly RogueMap<MapCell> map;

        public PickUp(GameObject item, RogueMap<MapCell> map) {
            this.item = item;
            this.item = item;
            this.map = map;
        }

        public ActionResult Perform(Actor actor, bool defaultAction = false) {
            actor.Inventory.Add(item);
            map.GameObjects.Remove((GameObject)item);

            if (item.Type == GameObjectType.Weapon) {
                Locator.Audio.PlaySound("weaponPickup");
            }

            return ActionResult.Succeed($"Picked up {item.Name}", true);
        }
    }
}
