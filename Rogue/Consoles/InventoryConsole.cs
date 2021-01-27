using Rogue.GameObjects;
using SadConsole;
using System;

namespace Rogue.Consoles {
    public class InventoryConsole : SadConsole.Console {
        private readonly Actor player;

        public InventoryConsole(Actor player) : base(60, 40) {
            this.Position = new SadRogue.Primitives.Point(1, 1);
            this.player = player;
        }

        public override void Update(TimeSpan delta) {
            this.Clear();

            Cursor.Position = new SadRogue.Primitives.Point(0, 0);
            Cursor.Print("Inventory");

            foreach (var item in player.Inventory) {
                Cursor.NewLine();
                Cursor.Print(item.Name);
            }

            base.Update(delta);
        }
    }
}
