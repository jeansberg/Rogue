using SadConsole;
using System;

namespace Rogue.Consoles {
    public class MenuConsole : SadConsole.Console {
        private string message;

        public MenuConsole(int width, int height) : base(width, height) {
            Position = new SadRogue.Primitives.Point(0, 0);
        }

        public void SetMessage(string message) {
            this.message = message;
        }

        public override void Update(TimeSpan delta) {
            this.Clear();

            Cursor.Position = new SadRogue.Primitives.Point(Width / 2, Height / 2);
            Cursor.Print(message);

            base.Update(delta);
        }
    }
}
