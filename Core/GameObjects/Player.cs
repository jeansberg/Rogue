using Core;
using Core.GameObjects;
using Core.Interfaces;
using Rogue.Services;
using System;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Player : Actor {
        private readonly string name;
        private int StepCount;
        private int experience;

        public (int number, int maxHealth, string name) Level { get {
                if (GetExperience() < 50) {
                    return (1, 10, "Guild Novice");
                }
                else if (GetExperience() < 150) {
                    return (2, 15, "Apprentice");
                }
                else if (GetExperience() < 450) {
                    return (3, 20, "Journeyman");
                }
                else if (GetExperience() < 1350) {
                    return (4, 25, "Adventurer");
                }
                else {
                    return (5, 30, "Fighter");
                }

            }}

        public Player(Point location, IFov fov, string name = "Player") : base(location, 10, fov) {
            this.name = name;
            SetExperience(0);
        }

        public override void Update() {
            StepCount++;

            if (StepCount == 21 - (Level.number * 2)) {
                RegenerateHealth();
                StepCount = 0;
            }
        }

        private void RegenerateHealth() {
            Health = Health < MaxHealth ? Health + 1 : Health;
        }

        public int GetExperience() { return experience; }
        public void SetExperience(int value) {
            var oldLevel = Level;
            experience = value;
            if (Level.number > oldLevel.number) {
                MaxHealth = Level.maxHealth;
                Health = MaxHealth;
                Locator.Audio.PlaySound("levelUp");
            }
        }

        public bool IsMaxLevel() {
            return Level.number == 5;
        }

        public Weapon? Weapon { get; set; }

        public override Color Color() {
            return System.Drawing.Color.Yellow;
        }

        public override int GlyphId() {
            return 64;
        }

        public override string Name() {
            return name;
        }

        public int? GetXpRequirementNextLevel() {
            return Level.number switch {
                1 => 50,
                2 => 150,
                3 => 450,
                4 => 1350,
                _ => null
            };
        }
    }}

