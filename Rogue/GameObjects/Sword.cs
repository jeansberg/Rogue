using Rogue.Actions;
using Rogue.MazeGenerator;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Spear : GameObject{
        public Spear(Point location) : base(location, Color.Yellow, 179, "Spear", GameObjectType.Weapon) {
        }

        public override IAction GetAction(RogueMap<MapCell> map, Direction.Types from) {
            return new PickUp(this, map);
        }
    }
}
