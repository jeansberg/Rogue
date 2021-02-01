using Rogue.Consoles;
using Rogue.GameObjects;
using Rogue.MazeGenerator;
using Rogue.Services;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace Rogue {
    class Program
    {
        private const int Width = 60;
        private const int Height = 40;
        private static RogueMap<MapCell> map;
        private static List<Actor> actors;

        static void Main(string[] args)
        {
            Locator.RegisterAudioPlayer(new AudioPlayer());

            var generator = new MapGenerator(new Map.RoomDecorator(), Width, Height, 100, 3, 9, 3, 9);

            map = generator.GenerateMap();
            
            var player = new Player(new Point(0, 0), Color.Yellow, new RogueSharp.FieldOfView<MapCell>(map));
            player.Inventory.Add(new Sword(new Point()));
            player.Inventory.Add(new Spear(new Point()));

            actors = new List<Actor> {
                player,
            };
            actors.AddRange(map.Actors);

            // Setup the engine and create the main window.
            SadConsole.Game.Create((int)(Width * 2) + 20 + 2, Height + 2);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.Instance.OnStart = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init() {
            // Any startup code for your game. We will use an example console for now
            var mapConsole = new MapConsole(map, false);
            var logConsole = new LogConsole();
            var messageConsole = new MessageConsole();
            var inventory = new InventoryConsole(actors.Single(a => a is Player));
            var mainConsole = new MainConsole(mapConsole, logConsole, messageConsole, inventory, actors);

            mainConsole.Children.Add(mapConsole);
            mainConsole.Children.Add(logConsole);
            mainConsole.Children.Add(messageConsole);
            mainConsole.Children.Add(inventory);

            SadConsole.Game.Instance.Screen = mainConsole;
        }
    }
}
