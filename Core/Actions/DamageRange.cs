using Rogue.GameObjects;
using Rogue.Services;
using System;

namespace Rogue {

    public class DamageRange {

        private Actor target;
        private readonly Actor attacker;
        private Missile? missile;

        public DamageRange(Actor target, Actor attacker, Missile? missile) {
            this.target = target;
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
                    MonsterType.Jackal => diceRoller.RollDice(1, 2),
                    MonsterType.Kobold => diceRoller.RollDice(1, 4),
                    MonsterType.Snake => diceRoller.RollDice(1, 3),
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            };
        }

        private int CalculatePlayerDamage(Player p, Missile? missile) {
            var diceRoller = Locator.Dice;
            var weaponDamage = p.Weapon?.WeaponType switch {
                WeaponType.Mace => missile != null ? diceRoller.RollDice(1, 3) : diceRoller.RollDice(2, 4),
                WeaponType.LongSword => missile != null ? diceRoller.RollDice(1, 2) : diceRoller.RollDice(3, 4),
                WeaponType.Arrow => missile != null ? diceRoller.RollDice(2, 3) : diceRoller.RollDice(1, 1),
                _ => diceRoller.RollDice(1, 2),
            };

            return weaponDamage * (1 + p.Level / 10);
        }
    }
}