using Core;
using Core.Interfaces;
using Rogue.Actions;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Spear : GameObject{
        public Spear(Point location) : base(location, Color.Yellow, 179, "Spear", GameObjectType.Weapon) {
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new PickUp(this, map);
        }
    }
}
