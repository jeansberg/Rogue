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
            WeaponType.Longsword => 47,
            WeaponType.Arrow => 196,
            WeaponType.Crossbow => 125,
            WeaponType.CrossbowBolt => 45,
            WeaponType.Dagger => 126,
            WeaponType.Dart => 16,
            WeaponType.Rock => 7,
            WeaponType.Shortbow => 41,
            WeaponType.Sling => 235,
            WeaponType.Shuriken => 42,
            WeaponType.Spear => 24,
            WeaponType.TwoHandedSword => 197,
            WeaponType.Staff => 244,
            WeaponType.Wand => 191,
            _ => throw new System.NotImplementedException(),
        };

        public override Color Color() => WeaponType switch {
            WeaponType.Arrow => System.Drawing.Color.SaddleBrown,
            WeaponType.Crossbow => System.Drawing.Color.SaddleBrown,
            WeaponType.CrossbowBolt => System.Drawing.Color.SaddleBrown,
            WeaponType.Dagger => System.Drawing.Color.LightSteelBlue,
            WeaponType.Dart => System.Drawing.Color.SaddleBrown,
            WeaponType.Longsword => System.Drawing.Color.LightSteelBlue,
            WeaponType.Mace => System.Drawing.Color.LightSteelBlue,
            WeaponType.Rock => System.Drawing.Color.LightSteelBlue,
            WeaponType.Shortbow => System.Drawing.Color.SaddleBrown,
            WeaponType.Sling => System.Drawing.Color.SaddleBrown,
            WeaponType.Shuriken => System.Drawing.Color.LightSteelBlue,
            WeaponType.Spear => System.Drawing.Color.LightSteelBlue,
            WeaponType.TwoHandedSword => System.Drawing.Color.LightSteelBlue,
            WeaponType.Staff => System.Drawing.Color.Brown,
            WeaponType.Wand => System.Drawing.Color.Brown,
            _ => throw new System.NotImplementedException(),
        };

        public override string Name() => WeaponType switch {
            WeaponType.Mace => "Mace",
            WeaponType.Longsword => "Longsword",
            WeaponType.Arrow => "Arrow",
            WeaponType.Crossbow => "Crossbow",
            WeaponType.CrossbowBolt => "Crossbow bolt",
            WeaponType.Dagger => "Dagger",
            WeaponType.Dart => "Dart",
            WeaponType.Rock => "Rock",
            WeaponType.Shortbow => "Shortbow",
            WeaponType.Sling => "Sling",
            WeaponType.Shuriken => "Shuriken",
            WeaponType.Spear => "Spear",
            WeaponType.TwoHandedSword => "Two-handed sword",
            WeaponType.Staff => "Staff",
            WeaponType.Wand => "Wand",
            _ => throw new System.NotImplementedException(),
        };

        public static int DungeonLevelMin(WeaponType type) => type switch {
            WeaponType.Mace => 1,
            WeaponType.Longsword => 5,
            WeaponType.Arrow => 1,
            WeaponType.Crossbow => 5,
            WeaponType.CrossbowBolt => 1,
            WeaponType.Dagger => 1,
            WeaponType.Dart => 1,
            WeaponType.Rock => 1,
            WeaponType.Shortbow => 1,
            WeaponType.Sling => 1,
            WeaponType.Shuriken => 5,
            WeaponType.Spear => 1,
            WeaponType.TwoHandedSword => 5,
            WeaponType.Staff => 1,
            WeaponType.Wand => 1,
            _ => throw new System.NotImplementedException(),
        };

        public override IAction GetAction(IMap map, Direction from) {
            return new PickUp(this, map);
        }

        public override void Update() {
        }
    }
}
