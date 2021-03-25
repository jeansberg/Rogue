using Core;
using Core.Interfaces;
using Rogue.Services;
using System;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Monster : Actor {
        public Monster(MonsterType type, Point location, IFov fov) : 
            base(location, GetStartingHealth(type), fov) {
            MonsterType = type;
        }

        private static int GetStartingHealth(MonsterType type) {
            int hitDice = type switch {
                MonsterType.Bat => 1,
                MonsterType.Hobgoblin => 1,
                MonsterType.Orc => 1,
                MonsterType.Jackal => 1,
                MonsterType.Kobold => 1,
                MonsterType.Snake => 1,
                _ => throw new NotImplementedException(),
            };

            return Locator.Dice.RollDice(hitDice, 8);
        }

        public MonsterType MonsterType { get; private set; }

        public WeaponType WeaponType { get; private set; }

        public override int GlyphId() => MonsterType switch {
            MonsterType.Hobgoblin => 104,
            MonsterType.Bat => 98,
            MonsterType.Orc => 79,
            MonsterType.Jackal => 106,
            MonsterType.Kobold => 107,
            MonsterType.Snake => 115,
            _ => throw new NotImplementedException(),
        };

        public override Color Color() => System.Drawing.Color.Green;

        public override string Name() => MonsterType switch {
            MonsterType.Hobgoblin => "Hobgoblin",
            MonsterType.Bat => "Bat",
            MonsterType.Orc => "Orc",
            MonsterType.Jackal => "Jackal",
            MonsterType.Kobold => "Kobold",
            MonsterType.Snake => "Snake",
            _ => throw new System.NotImplementedException(),
        };

        public static (int MinLevel, int MaxLevel) LevelRange(MonsterType type) => type switch {
            MonsterType.Bat => (1, 8),
            MonsterType.Hobgoblin => (1, 10),
            MonsterType.Orc => (4, 13),
            MonsterType.Jackal => (1, 7),
            MonsterType.Kobold => (1, 6),
            MonsterType.Snake => (1, 9),
            _ => throw new System.NotImplementedException(),
        };
    }
}
