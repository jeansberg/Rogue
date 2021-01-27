using Rogue.GameObjects;
using Rogue.MazeGenerator;

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

            return ActionResult.Succeed($"Picked up {item.Name}", true);
        }
    }
}
