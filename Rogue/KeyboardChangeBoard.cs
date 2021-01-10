using Rogue.Actors;
using Rogue.MazeGenerator;
using RogueSharp;
using SadConsole;
using SadConsole.Input;

namespace Rogue {
    public class KeyboardChangeBoard : SadConsole.Components.KeyboardConsoleComponent {
        private Map<MapCell> map;
        private Player player;

        public KeyboardChangeBoard(Map<MapCell> map, Player player) {
            this.map = map;
            this.player = player;
        }

        public override void ProcessKeyboard(IScreenObject host, SadConsole.Input.Keyboard keyboard, out bool handled) {
            if (keyboard.IsKeyPressed(Keys.Down)) {
                player.Location = new Point(0, 1);
            }
            handled = true;
        }
    }
}
