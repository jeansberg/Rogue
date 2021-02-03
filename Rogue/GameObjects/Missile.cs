using Rogue.Actions;
using Rogue.MazeGenerator;
using SadRogue.Primitives;
using System.Windows.Forms;

namespace Rogue.GameObjects {
    public class Missile : GameObject {
        public Missile(Point location) : base(location, Color.Brown, 45, "missile", GameObjectType.Missile) {
        }

        public Orientation Orientation { get; set; }

        public override IAction GetAction(RogueMap<MapCell> map, Direction.Types from) {
            return new NullAction();
        }
    }
}
