using System;
using SadConsole;

namespace Rogue.Graphics {
    public class LogConsole : SadConsole.Console {
        public LogConsole() : base(20, 40) {}

        public override void Update(TimeSpan delta) {
            this.Clear();

            this.Position = new SadRogue.Primitives.Point(60 + 2, 1);
            Cursor.Position = new SadRogue.Primitives.Point(0, 0);
            Cursor.Print("Log");

            base.Update(delta);
        }
    }
}
