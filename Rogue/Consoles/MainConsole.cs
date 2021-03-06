﻿using Rogue.Components;
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
        private readonly List<Actor> actors;
        private readonly Actor player;
        private readonly KeyboardHandler keyboardHandlerObject;

        public MainConsole(MapConsole mapConsole, LogConsole logConsole, MessageConsole messageConsole, InventoryConsole inventory, List<Actor> actors) : base(80, 40) {
            this.mapConsole = mapConsole;
            this.logConsole = logConsole;
            this.messageConsole = messageConsole;
            this.inventory = inventory;
            this.actors = actors;
            player = actors.Single(a => a is Player);

            this.Font = new Font(12, 12, 0, 16, 16, 0, new GameTexture(new SFML.Graphics.Texture("../../../Cheepicus_12x12.png")), "mapFont");

            keyboardHandlerObject = new KeyboardHandler(mapConsole, logConsole, messageConsole, player, inventory, actors);
            GameHost.Instance.FocusedScreenObjects.Set(this);

            SadComponents.Add(keyboardHandlerObject);

            inventory.IsVisible = false;
        }

        public override void Update(TimeSpan delta) {
            mapConsole.Update(this.actors, delta);
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
