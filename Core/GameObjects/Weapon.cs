using Core.Interfaces;
using Rogue;
using Rogue.Actions;
using Rogue.GameObjects;
using System.Drawing;

namespace Core.GameObjects {
    public class Weapon : GameObject {
        public Weapon(Point location, WeaponType weaponType) : 
            base(location) {
            this.WeaponType = weaponType;
        }

        public WeaponType WeaponType { get; private set; }

        public override int GlyphId() => WeaponType switch {
            WeaponType.Mace => 173,
            WeaponType.LongSword => 47,
            WeaponType.Arrow => 26,
            _ => throw new System.NotImplementedException(),
        };

        public override Color Color() => System.Drawing.Color.LightSteelBlue;

        public override string Name() => WeaponType switch {
            WeaponType.Mace => "Mace",
            WeaponType.LongSword => "Longsword",
            WeaponType.Arrow => "Arrow",
            _ => throw new System.NotImplementedException(),
        };

        public override IAction GetAction(IMap map, Direction from) {
            return new PickUp(this, map);
        }

        public override void Update() {
        }
    }
}
