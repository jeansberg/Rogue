using System;
using Rogue.GameObjects;
using SadConsole;

namespace Rogue.Consoles {
    public class StatusConsole : SadConsole.Console {
        private readonly Player player;

        public StatusConsole(GameObjects.Player player) : base(60, 3) {
            Position = new SadRogue.Primitives.Point(1, 30 + 1);
            this.player = player;
        }

        public override void Update(TimeSpan delta) {
            this.Clear();

            Cursor.Position = new SadRogue.Primitives.Point(0, 0);
            PrintHealth();
            PrintLevel();

            PrintWeapon();
            PrintArmor();

            base.Update(delta);
        }

        private void PrintLevel() {
            Cursor.Print(player.Level.name);
            Cursor.NewLine();
            Cursor.Print(player.GetExperience().ToString());
            if (!player.IsMaxLevel()) {
                Cursor.Print($" / {player.GetXpRequirementNextLevel()}");
            }
        }

        private void PrintHealth() {
            Cursor.Print($"Health: {player.Health}");
            Cursor.NewLine();
        }

        private void PrintWeapon() {
            Cursor.Position = new SadRogue.Primitives.Point(20, 0);
            Cursor.Print($"Weapon: {player.Weapon?.Name()}");
            Cursor.NewLine();
        }

        private void PrintArmor() {
            Cursor.Position = new SadRogue.Primitives.Point(20, 1);
            Cursor.Print($"Armor: ");
            Cursor.NewLine();
        }
    }
}
