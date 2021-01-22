using System;
using SadConsole;

namespace Rogue.Display {
    public class MessageConsole : SadConsole.Console {
        private string message = "";

        public MessageConsole() : base(40, 1) {
            this.Position = new SadRogue.Primitives.Point(0, 0);
        }

        public void SetMessage(string message) {
            this.message = message;
        }

        public override void Update(TimeSpan delta) {
            this.Clear();

            Cursor.Position = new SadRogue.Primitives.Point(0, 0);
            Cursor.Print(message);

            base.Update(delta);
        }
    }
}
