using Rogue.Components;
using Rogue.GameObjects;
using SadConsole;
using SadConsole.Host;
using SadConsole.Input;
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
        private readonly KeyboardHandler keyboardHandlerObject;

        public MainConsole(MapConsole mapConsole, LogConsole logConsole, MessageConsole messageConsole, KeyboardHandler keyboardHandler, InventoryConsole inventory) : base(80, 40) {
            this.mapConsole = mapConsole;
            this.logConsole = logConsole;
            this.messageConsole = messageConsole;
            this.inventory = inventory;


            this.Font = new Font(12, 12, 0, 16, 16, 0, new GameTexture(new SFML.Graphics.Texture("../../../Cheepicus_12x12.png")), "mapFont");

            GameHost.Instance.FocusedScreenObjects.Set(this);

            keyboardHandlerObject = keyboardHandler;
            SadComponents.Add(keyboardHandlerObject);

            inventory.IsVisible = false;
        }

        public override void Update(TimeSpan delta) {
            mapConsole.Update(delta);
            messageConsole.Update(delta);
            logConsole.Update(delta);
            inventory.Update(delta);

            keyboardHandlerObject.Update(this, delta);
        }

        public override bool ProcessMouse(MouseScreenObjectState state) {
            keyboardHandlerObject.ProcessMouse(this, state, out bool handled);
            return handled;
        }
    }
}
