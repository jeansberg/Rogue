using Rogue.GameObjects;
using Rogue.Services;
using System;

namespace Rogue {

    public class DamageRange {
        private readonly Actor attacker;
        private Missile? missile;

        public DamageRange(Actor target, Actor attacker, Missile? missile) {
            this.attacker = attacker;
            this.missile = missile;
        }

        public int GetDamage() {
            var diceRoller = Locator.Dice;
            return attacker switch {
                Player p => CalculatePlayerDamage(p, missile),
                Monster m => m.MonsterType switch {
                    MonsterType.Bat => diceRoller.RollDice(1, 2),
                    MonsterType.Hobgoblin => diceRoller.RollDice(1, 8),
                    MonsterType.Orc => diceRoller.RollDice(1, 8),
                    MonsterType.Kestrel => diceRoller.RollDice(1, 2),
                    MonsterType.IceMonster => diceRoller.RollDice(1, 2),
                    MonsterType.Snake => diceRoller.RollDice(1, 3),
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            };
        }

        private int CalculatePlayerDamage(Player p, Missile? missile) {
            var diceRoller = Locator.Dice;
            (int rangedDamage, int meleeDamage) = p.Weapon?.WeaponType switch {
                WeaponType.Mace => (diceRoller.RollDice(1, 3), diceRoller.RollDice(2, 4)),
                WeaponType.Longsword => (diceRoller.RollDice(1, 2), diceRoller.RollDice(3, 4)),
                WeaponType.Arrow => (diceRoller.RollDice(2, 3), diceRoller.RollDice(1, 1)),
                WeaponType.Crossbow => (diceRoller.RollDice(1, 1), diceRoller.RollDice(1, 1)),
                WeaponType.CrossbowBolt => (diceRoller.RollDice(1, 2), diceRoller.RollDice(1, 10)),
                WeaponType.Dagger => (diceRoller.RollDice(1, 6), diceRoller.RollDice(1, 4)),
                WeaponType.Dart => (diceRoller.RollDice(1, 1), diceRoller.RollDice(1, 3)),
                WeaponType.Rock => (diceRoller.RollDice(1, 2), diceRoller.RollDice(1, 4)),
                WeaponType.Shortbow => (diceRoller.RollDice(1, 1), diceRoller.RollDice(1, 1)),
                WeaponType.Sling => (diceRoller.RollDice(0, 0), diceRoller.RollDice(0, 0)),
                WeaponType.Shuriken => (diceRoller.RollDice(1, 2), diceRoller.RollDice(2, 4)),
                WeaponType.Spear => (diceRoller.RollDice(1, 8), diceRoller.RollDice(1, 6)),
                WeaponType.TwoHandedSword => (diceRoller.RollDice(4, 4), diceRoller.RollDice(1, 2)),
                WeaponType.Staff => (diceRoller.RollDice(2, 3), diceRoller.RollDice(1, 1)),
                WeaponType.Wand => (diceRoller.RollDice(1, 1), diceRoller.RollDice(1, 1)),
                _ => (diceRoller.RollDice(0, 0), diceRoller.RollDice(1, 2)),
            };

            if (missile != null) {
                return rangedDamage;
            }
            else {
                return meleeDamage;
            }
        }
    }
}