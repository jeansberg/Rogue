using System;
using System.Collections.Generic;
using System.Drawing;
using Rogue.GameObjects;
using SadConsole;
using Utilities.SadConsole;

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

            Cursor.Position = new SadRogue.Primitives.Point(20, 0);
            PrintWeapon();
            PrintArmor();

            base.Update(delta);
        }

        private void PrintLevel() {
            Cursor.Print($"Level:  {player.Level}");
            Cursor.NewLine();
        }

        private void PrintHealth() {
            Cursor.Print($"Health: {player.Health}");
            Cursor.NewLine();
        }

        private void PrintWeapon() {
            Cursor.Print($"Weapon: {player.Weapon?.Name()}");
            Cursor.NewLine();
        }

        private void PrintArmor() {
            Cursor.Print($"Armor: ");
            Cursor.NewLine();
        }
    }
}
