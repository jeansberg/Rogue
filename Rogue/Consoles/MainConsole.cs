using Rogue.Components;
using Rogue.GameObjects;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using Console = SadConsole.Console;

namespace Rogue.Consoles {
    public class MainConsole : Console {
        private readonly MapConsole mapConsole;
        private readonly LogConsole logConsole;
        private readonly MessageConsole messageConsole;
        private readonly InventoryConsole inventory;
        private readonly List<Actor> actors;
        private readonly Actor player;
        private readonly KeyboardHandler _keyboardHandlerObject;

        public MainConsole(MapConsole mapConsole, LogConsole logConsole, MessageConsole messageConsole, InventoryConsole inventory, List<Actor> actors) : base(80, 40) {
            this.mapConsole = mapConsole;
            this.logConsole = logConsole;
            this.messageConsole = messageConsole;
            this.inventory = inventory;
            this.actors = actors;
            player = actors.Single(a => a is Player);

            _keyboardHandlerObject = new KeyboardHandler(mapConsole, logConsole, messageConsole, player, inventory, actors);
            GameHost.Instance.FocusedScreenObjects.Set(this);

            SadComponents.Add(_keyboardHandlerObject);

            inventory.IsVisible = false;
        }

        public override void Update(TimeSpan delta) {
            mapConsole.Update(this.actors, delta);
            messageConsole.Update(delta);
            logConsole.Update(delta);
            inventory.Update(delta);
        }
    }
}
