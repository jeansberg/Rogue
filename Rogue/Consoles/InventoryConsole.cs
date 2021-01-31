using Rogue.GameObjects;
using SadConsole;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Consoles {
    public class InventoryConsole : SadConsole.UI.ControlsConsole {
        private readonly Actor player;
        private IEnumerable<SadConsole.Input.Keys> Keys;
        private Dictionary<Keys, GameObject> itemsWithKeys;

        public InventoryConsole(Actor player) : base(60, 40) {
            this.Position = new SadRogue.Primitives.Point(1, 1);
            this.player = player;

            Keys = new SadConsole.Input.Keys[] {
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

            itemsWithKeys = player.Inventory
                .Zip(Keys, (item, key) => (item, key))
                .ToDictionary(k => k.key, v => v.item);

            foreach (var item in itemsWithKeys) {
                Cursor.Print(new ColoredString($"{item.Value.Name} - " + item.Key, new ColoredString.ColoredGlyphEffect {
                    Decorators = new CellDecorator[] {new CellDecorator(SadRogue.Primitives.Color.Red, 178, Mirror.None) }
                }));
                Cursor.NewLine();
            }

            base.Update(delta);
        }

        public GameObject GetItem(Keys key) {
            if(itemsWithKeys.TryGetValue(key, out var item)) {
                return item;
            }

            return null;
        }
    }
}
