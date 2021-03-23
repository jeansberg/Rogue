using Core;
using Core.Interfaces;
using Rogue.Actions;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Missile : GameObject {
        public Missile(Point location) : base(location, Color.Brown, 45, "missile", GameObjectType.Missile) {
        }

        public Orientation Orientation { get; set; }

        public override IAction GetAction(IMap map, Direction from) {
            return new NullAction();
        }
    }
}
