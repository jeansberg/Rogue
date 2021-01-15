using Rogue.GameObjects;
using Rogue.Graphics;
using Rogue.MazeGenerator;
using RogueSharp;
using SadRogue.Primitives;
using System.Collections.Generic;

namespace Rogue {
    class Program
    {
        private const int Width = 60;
        private const int Height = 40;
        private static RogueMap<MapCell> map;
        private static Player player;

        static void Main(string[] args)
        {
            var generator = new MapGenerator(Width, Height, 50, 3, 9, 3, 9);

            map = generator.GenerateMap();

            player = new Player(new SadRogue.Primitives.Point(0, 0), Color.Yellow);

            var actors = new List<Actor> {
                player
            };

            // Setup the engine and create the main window.
            SadConsole.Game.Create(Width + 20 + 2, Height + 2);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.Instance.OnStart = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init() {
            // Any startup code for your game. We will use an example console for now
            var mapConsole = new MapConsole(map, player);
            var logConsole = new LogConsole();
            var mainConsole = new MainConsole();

            mainConsole.SadComponents.Add(new KeyboardChangeBoard(map, player));

            mainConsole.Children.Add(mapConsole);
            mainConsole.Children.Add(logConsole);



            SadConsole.Game.Instance.Screen = mainConsole;
        }
    }
}
