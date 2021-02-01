using Rogue.Actions;
using Rogue.MazeGenerator;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Sword : GameObject{
        public Sword(Point location) : base(location, Color.Blue, 47, "Sword", GameObjectType.Weapon) {
        }

        public override IAction GetAction(RogueMap<MapCell> map, Direction.Types from) {
            return new PickUp(this, map);
        }
    }
}
