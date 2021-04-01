using Core;
using Core.GameObjects;
using Core.Interfaces;
using System;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Player : Actor {
        private readonly string name;
        public int Experience { get; set; }
        public (int number, string name) Level { get {
                if (Experience < 50) {
                    return (1, "Guild Novice");
                }
                else if (Experience < 150) {
                    return (2, "Apprentice");
                }
                else if (Experience < 450) {
                    return (3, "Journeyman");
                }
                else if (Experience < 1350) {
                    return (4, "Adventurer");
                }
                else {
                    return (5, "Fighter");
                }

            }}

        public Player(Point location, IFov fov, string name = "Player") : base(location, 10, fov) {
            this.name = name;
            Experience = 0;
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
                1 => 0,
                2 => 50,
                3 => 150,
                4 => 450,
                5 => 1350,
                _ => null
            };
        }
    }}

