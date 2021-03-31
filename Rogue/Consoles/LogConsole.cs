using System;
using System.Collections.Generic;
using SadConsole;

namespace Rogue.Consoles {
    public class LogConsole : SadConsole.Console {
        private readonly Stack<string> messages;
        public LogConsole() : base(40, 40) {
            Position = new SadRogue.Primitives.Point(90 + 2, 1);
            messages = new Stack<string>();
        }

        public override void Update(TimeSpan delta) {
            this.Clear();

            Cursor.Position = new SadRogue.Primitives.Point(0, 0);
            Cursor.Print("Log");
            Cursor.NewLine();

            

            foreach (var message in messages) {
                Cursor.Print(message);
                if (Cursor.Column != 0) {
                    Cursor.NewLine();
                }
            }

            base.Update(delta);
        }

        public void Log(string message) {
            messages.Push(message);
        }
    }
}
