using Rogue.GameObjects;
using SadConsole;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities.SadConsole;

namespace Rogue.Consoles {
    public class InventoryConsole : SadConsole.UI.ControlsConsole {
        private readonly Actor player;
        private IEnumerable<Keys> Keys;
        private Dictionary<Keys, GameObject> itemsWithKeys;

        public InventoryConsole(Actor player) : base(60, 40) {
            this.Position = new SadRogue.Primitives.Point(1, 1);
            this.player = player;

            Keys = new Keys[] {
                SadConsole.Input.Keys.Q,
                SadConsole.Input.Keys.W,
                SadConsole.Input.Keys.E,
                SadConsole.Input.Keys.R,
                SadConsole.Input.Keys.T,
                SadConsole.Input.Keys.Y,
                SadConsole.Input.Keys.U,
                SadConsole.Input.Keys.I,
                SadConsole.Input.Keys.O,
                SadConsole.Input.Keys.P,
            };
        }

        public override void Update(TimeSpan delta) {
            this.Clear();

            Cursor.Position = new SadRogue.Primitives.Point(0, 0);
            Cursor.Print("Inventory");
            Cursor.NewLine();
            Cursor.NewLine();


            itemsWithKeys = player.Inventory
                .Zip(Keys, (item, key) => (item, key))
                .ToDictionary(k => k.key, v => v.item);

            foreach (var item in itemsWithKeys) {
                PrintItem(item);
                Cursor.NewLine();
            }

            if (itemsWithKeys.Count == 0) {
                Cursor.Print("Your inventory is empty!");
            }

            base.Update(delta);
        }

        private void PrintItem(KeyValuePair<Keys, GameObject> item) {
            var gameObject = item.Value;
            Cursor.Print(new ColoredString(new[] { new ColoredGlyph(gameObject.Color().ToSadColor(), Color.Black.ToSadColor(), item.Value.GlyphId()) }));
            Cursor.Print(" ");
            Cursor.Print($"{gameObject.Name()} - " + item.Key);
        }

        public GameObject GetItem(Keys key) {
            if(itemsWithKeys.TryGetValue(key, out var item)) {
                return item;
            }

            return null;
        }
    }
}
