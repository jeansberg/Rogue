using System;

namespace Rogue.Graphics {
    public class MainConsole : SadConsole.Console {
        public MainConsole() : base(80, 40) { }

        public override void Update(TimeSpan delta) {
            foreach(SadConsole.Console c in Children) {
                c.Update(delta);
            }
        }
    }
}
