using Rogue.Components;
using Rogue.GameObjects;
using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Console = SadConsole.Console;

namespace Rogue.Consoles {
    public class MainConsole : Console {
        private readonly MapConsole mapConsole;
        private readonly LogConsole logConsole;
        private readonly MessageConsole messageConsole;
        private readonly List<Actor> actors;
        private readonly Actor player;
        private readonly KeyboardHandler _keyboardHandlerObject;

        public MainConsole(MapConsole mapConsole, LogConsole logConsole, MessageConsole messageConsole, List<Actor> actors) : base(80, 40) {
            this.mapConsole = mapConsole;
            this.logConsole = logConsole;
            this.messageConsole = messageConsole;
            this.actors = actors;
            player = actors.Single(a => a is Player);

            _keyboardHandlerObject = new KeyboardHandler(mapConsole, logConsole, messageConsole, player, actors);
            GameHost.Instance.FocusedScreenObjects.Set(this);

            SadComponents.Add(_keyboardHandlerObject);
        }

        public override void Update(TimeSpan delta) {
            mapConsole.Update(this.actors, delta);
            messageConsole.Update(delta);
            logConsole.Update(delta);

        }
    }
}
