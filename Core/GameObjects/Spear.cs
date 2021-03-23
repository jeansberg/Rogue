using Core;
using Core.Interfaces;
using Rogue.Actions;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Sword : GameObject{
        public Sword(Point location) : base(location, Color.Blue, 47, "Sword", GameObjectType.Weapon) {
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new PickUp(this, map);
        }
    }
}
